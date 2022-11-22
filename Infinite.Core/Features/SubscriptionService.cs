using Hangfire;

namespace Infinite.Core.Features;

public class SubscriptionService : ISubscriptionService
{
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<UserSubscriptionResponse>> GetCurrentSubscription(string userId)
    {
        try
        {
            var subscription = await _unitOfWork.GetRepository<UserCurrentSubscription>().Entities
                .FirstAsync(x => x.UserId == userId);
            var response = new UserSubscriptionResponse()
            {
                ExpiresOn = subscription.ExpiresOn,
                SubscriptionBasis = subscription.SubscriptionBasis,
                SubscriptionDate = subscription.SubscriptionDate,
                SubscriptionPlan = subscription.SubscriptionPlan
            };
            return await Result<UserSubscriptionResponse>.SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await Result<UserSubscriptionResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult> UpdateSubscription(UpdateSubscriptionRequest request, string userId)
    {
        try
        {
            var currSub = await _unitOfWork.GetRepository<UserCurrentSubscription>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (currSub != null)
            {
                await _unitOfWork.GetRepository<UserCurrentSubscription>().DeleteAsync(currSub);
            }
            var newSub = new UserCurrentSubscription
            {
                Id = Guid.NewGuid().ToString(),
                SubscriptionBasis = request.SubscriptionBasis,
                SubscriptionPlan = request.SubscriptionPlan,
                SubscriptionDate = DateTime.Now,
                ExpiresOn = request.SubscriptionPlan switch
                {
                    SubscriptionPlan.Free => DateTime.MaxValue,
                    _ => DateTime.Now.AddMonths(request.SubscriptionBasis switch
                        {
                            SubscriptionBasis.Monthly => 1,
                            SubscriptionBasis.Yearly => 12,
                            _ => DateTime.MaxValue.Month
                        })
                },
                UserId = userId
            };
            await _unitOfWork.GetRepository<UserCurrentSubscription>().AddAsync(newSub);
            await _unitOfWork.Commit();
            if (newSub.SubscriptionPlan != SubscriptionPlan.Free)
            {
                BackgroundJob.Schedule(() => RevertSubscription(userId), newSub.ExpiresOn);
            }
            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    private async Task RevertSubscription(string userId)
    {
        var currSub = await _unitOfWork.GetRepository<UserCurrentSubscription>().Entities
            .FirstAsync(x => x.UserId == userId);
        currSub.ExpiresOn = DateTime.MaxValue;
        currSub.SubscriptionBasis = SubscriptionBasis.None;
        currSub.SubscriptionPlan = SubscriptionPlan.Free;
        await _unitOfWork.GetRepository<UserCurrentSubscription>().UpdateAsync(currSub, currSub.Id);
        await _unitOfWork.Commit();
    }
}