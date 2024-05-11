using Hangfire;

namespace Infinite.Core.Features;

public class SubscriptionService(IUnitOfWork unitOfWork) : ISubscriptionService
{
    public async Task<IResult<UserSubscriptionResponse>> GetCurrentSubscription(string userId)
    {
        try
        {
            var subscription = await unitOfWork.GetRepository<UserCurrentSubscription>().Entities
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
            var currSub = await unitOfWork.GetRepository<UserCurrentSubscription>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (currSub != null)
            {
                await unitOfWork.GetRepository<UserCurrentSubscription>().DeleteAsync(currSub);
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
            await unitOfWork.GetRepository<UserCurrentSubscription>().AddAsync(newSub);
            await unitOfWork.Commit();
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
        var currSub = await unitOfWork.GetRepository<UserCurrentSubscription>().Entities
            .FirstAsync(x => x.UserId == userId);
        currSub.ExpiresOn = DateTime.MaxValue;
        currSub.SubscriptionBasis = SubscriptionBasis.None;
        currSub.SubscriptionPlan = SubscriptionPlan.Free;
        await unitOfWork.GetRepository<UserCurrentSubscription>().UpdateAsync(currSub, currSub.Id);
        await unitOfWork.Commit();
    }
}