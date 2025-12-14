using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameOverDose.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    release = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    tba = table.Column<bool>(type: "boolean", nullable: false),
                    background_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    rating_top = table.Column<int>(type: "integer", nullable: true),
                    ratings = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ratings_top = table.Column<int>(type: "integer", nullable: true),
                    ratings_count = table.Column<int>(type: "integer", nullable: true),
                    rewiews_text_count = table.Column<int>(type: "integer", nullable: true),
                    added = table.Column<int>(type: "integer", nullable: true),
                    added_by_status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    metacritics = table.Column<int>(type: "integer", nullable: true),
                    playtime = table.Column<int>(type: "integer", nullable: true),
                    suggestions_count = table.Column<int>(type: "integer", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    esrb_rating = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    platforms = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    lvl = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    commentid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    rating = table.Column<int>(type: "integer", nullable: true),
                    gameid = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.commentid);
                    table.ForeignKey(
                        name: "FK_comment_games_gameid",
                        column: x => x.gameid,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "friend",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid1 = table.Column<int>(type: "integer", nullable: false),
                    userid2 = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friend", x => x.id);
                    table.CheckConstraint("CK_Friend_DifferentUsers", "userid1 <> userid2");
                    table.ForeignKey(
                        name: "FK_friend_users_userid1",
                        column: x => x.userid1,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_friend_users_userid2",
                        column: x => x.userid2,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usergame",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    gameid = table.Column<int>(type: "integer", nullable: false),
                    hours = table.Column<int>(type: "integer", nullable: false),
                    added_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_played = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    progress = table.Column<int>(type: "integer", nullable: false),
                    personal_rating = table.Column<int>(type: "integer", nullable: true),
                    is_favorite = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usergame", x => x.id);
                    table.ForeignKey(
                        name: "FK_usergame_games_gameid",
                        column: x => x.gameid,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usergame_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_comment_gameid",
                table: "comment",
                column: "gameid");

            migrationBuilder.CreateIndex(
                name: "idx_comment_userid",
                table: "comment",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "idx_friend_userid1",
                table: "friend",
                column: "userid1");

            migrationBuilder.CreateIndex(
                name: "idx_friend_userid2",
                table: "friend",
                column: "userid2");

            migrationBuilder.CreateIndex(
                name: "idx_friend_users",
                table: "friend",
                columns: new[] { "userid1", "userid2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_games_rating",
                table: "games",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "idx_games_slug",
                table: "games",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_usergame_gameid",
                table: "usergame",
                column: "gameid");

            migrationBuilder.CreateIndex(
                name: "idx_usergame_user_game",
                table: "usergame",
                columns: new[] { "userid", "gameid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_usergame_userid",
                table: "usergame",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "idx_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_users_nickname",
                table: "users",
                column: "nickname",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "friend");

            migrationBuilder.DropTable(
                name: "usergame");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
