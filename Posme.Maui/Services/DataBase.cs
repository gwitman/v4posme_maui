﻿using SQLite;

namespace Posme.Maui.Services;

public class DataBase
{
    public SQLiteAsyncConnection Database;

    public DataBase()
    {
        _ = Init();
    }
    private async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(ConstantsSqlite.DatabasePath, ConstantsSqlite.Flags);
        await CreateTableUser();
    }

    private async Task CreateTableUser()
    {
        var query = """
                    create table if not exists tb_user
                    (
                        companyID             int          default 0   not null,
                        branchID              int          default 0   not null,
                        userID                int auto_increment
                            primary key,
                        nickname              varchar(250)             null,
                        password              varchar(250)             null,
                        createdOn             varchar(250)             null,
                        isActive              tinyint(1)               null,
                        email                 varchar(250) default '0' not null,
                        createdBy             int          default 0   not null,
                        employeeID            int          default 0   not null,
                        useMobile             int          default 0   not null,
                        phone                 varchar(255)             null,
                        lastPayment           datetime                 null,
                        comercio              varchar(255)             null,
                        foto                  varchar(255)             null,
                        token_google_calendar varchar(1200)            null
                    )
                    """;
        await Database.ExecuteAsync(query);
    }
}