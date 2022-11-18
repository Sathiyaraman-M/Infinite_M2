using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infinite.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserFollows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFollows",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FollowedId = table.Column<string>(type: "TEXT", nullable: true),
                    FollowedProfileInfoId = table.Column<string>(type: "TEXT", nullable: true),
                    FollowerId = table.Column<string>(type: "TEXT", nullable: true),
                    FollowerProfileInfoId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollows_AspNetUsers_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollows_AspNetUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollows_UserProfileInfos_FollowedProfileInfoId",
                        column: x => x.FollowedProfileInfoId,
                        principalTable: "UserProfileInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollows_UserProfileInfos_FollowerProfileInfoId",
                        column: x => x.FollowerProfileInfoId,
                        principalTable: "UserProfileInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowedId",
                table: "UserFollows",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowedProfileInfoId",
                table: "UserFollows",
                column: "FollowedProfileInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerProfileInfoId",
                table: "UserFollows",
                column: "FollowerProfileInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFollows");
        }
    }
}
