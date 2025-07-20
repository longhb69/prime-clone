using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActor_Moives_MovieId",
                table: "MovieActor");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCountry_Moives_MoviesId",
                table: "MovieCountry");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirector_Moives_MovieId",
                table: "MovieDirector");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Moives_MoviesId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieWriter_Moives_MovieId",
                table: "MovieWriter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Moives",
                table: "Moives");

            migrationBuilder.RenameTable(
                name: "Moives",
                newName: "Movies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActor_Movies_MovieId",
                table: "MovieActor",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCountry_Movies_MoviesId",
                table: "MovieCountry",
                column: "MoviesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirector_Movies_MovieId",
                table: "MovieDirector",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MoviesId",
                table: "MovieGenres",
                column: "MoviesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieWriter_Movies_MovieId",
                table: "MovieWriter",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActor_Movies_MovieId",
                table: "MovieActor");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCountry_Movies_MoviesId",
                table: "MovieCountry");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirector_Movies_MovieId",
                table: "MovieDirector");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MoviesId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieWriter_Movies_MovieId",
                table: "MovieWriter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "Moives");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Moives",
                table: "Moives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActor_Moives_MovieId",
                table: "MovieActor",
                column: "MovieId",
                principalTable: "Moives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCountry_Moives_MoviesId",
                table: "MovieCountry",
                column: "MoviesId",
                principalTable: "Moives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirector_Moives_MovieId",
                table: "MovieDirector",
                column: "MovieId",
                principalTable: "Moives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Moives_MoviesId",
                table: "MovieGenres",
                column: "MoviesId",
                principalTable: "Moives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieWriter_Moives_MovieId",
                table: "MovieWriter",
                column: "MovieId",
                principalTable: "Moives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
