using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopping_project.DataAccess.Migrations
{
    public partial class AddStoreProcedureToCoverTypetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE Sp_Covertypes_Create
	
	                                @Name varchar(50)
                                AS
	                                insert CoverTypes values(@Name)");


            migrationBuilder.Sql(@"CREATE PROCEDURE Sp_Covertypes_Update
                                   	@id int,
	                                @Name varchar(50)
                                AS
                                    update CoverTypes set Name=@Name where id=@id ");

            migrationBuilder.Sql(@"CREATE PROCEDURE Sp_Covertypes_Delete
                                   	@id int
                                AS
                                   delete CoverTypes where id=@id ");

            migrationBuilder.Sql(@"CREATE PROCEDURE Sp_Covertypes_GetCoverTypes
                                   
                                AS
                                    select* from CoverTypes ");

            migrationBuilder.Sql(@"CREATE PROCEDURE Sp_Covertypes_GetCoverType
                                   	@id int
	                                
                                AS
                                    select *from CoverTypes where @id=id ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Drop Procedure Sp_Covertypes_GetCoverType ");
            migrationBuilder.Sql(@"Drop Procedure Sp_Covertypes_GetCoverTypes ");
            migrationBuilder.Sql(@"Drop Procedure Sp_Covertypes_Create ");
            migrationBuilder.Sql(@"Drop Procedure Sp_Covertypes_Update ");
            migrationBuilder.Sql(@"Drop Procedure Sp_Covertypes_Delete ");

        }
    }
}
