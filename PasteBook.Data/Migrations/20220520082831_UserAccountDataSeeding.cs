using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Linq;

namespace PasteBook.Data.Migrations
{
    public partial class UserAccountDataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var faker = new Faker("en");
            foreach (int num in Enumerable.Range(1, 100))
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();
                migrationBuilder.InsertData(
                    table: "UserAccounts",
                    columns: new[] { "FirstName", "LastName", "UserName", "EmailAddress", "Password", "Birthday", "MobileNumber", "Active", "CreatedDate" },
                    values: new object[] { firstName, lastName, (firstName + lastName + num), faker.Internet.Email(), faker.Internet.Password(12), faker.Person.DateOfBirth, faker.Phone.PhoneNumber("63##########"), true, DateTime.Now }
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
