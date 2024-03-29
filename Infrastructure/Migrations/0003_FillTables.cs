﻿
using FluentMigrator;

[Migration(3)]
public class FillTables: ForwardOnlyMigration
{
    public override void Up()
    {
        Execute.Sql(@"
            INSERT INTO Department (Name, Phone)
            VALUES
            ('Разработка', '+71234567890'),
            ('Маркетинг', '+70987654321'),
            ('Отдел кадров', '+70223344556'),
            ('Финансовый отдел', '+70887766554'),
            ('ИТ поддержка', '+70111222333'),
            ('Логистика', '+70444555666'),
            ('Служба безопасности', '+70777888999'),
            ('Исследования и разработки', '+70333222111'),
            ('Управление качеством', '+70555666777'),
            ('Клиентский сервис', '+70666888999')
            ON CONFLICT DO NOTHING;

            INSERT INTO Employee (Name, Surname, Phone, CompanyId, Type, Number, DepartmentId)
            VALUES
            ('Иван', 'Иванов', '+71234567890', 1, 'Паспорт', '4001', 1),
            ('Петр', 'Петров', '+70987654321', 1, 'Паспорт', '4002', 2),
            ('Сергей', 'Сергеев', '+70223344556', 2, 'Паспорт', '4003', 3),
            ('Алексей', 'Алексеев', '+70887766554', 2, 'Паспорт', '4004', 4),
            ('Мария', 'Мариева', '+70111222333', 3, 'Паспорт', '4005', 5),
            ('Ольга', 'Ольгина', '+70444555666', 3, 'Паспорт', '4006', 6),
            ('Наталья', 'Натальева', '+70777888999', 4, 'Паспорт', '4007', 7),
            ('Дмитрий', 'Дмитриев', '+70333222111', 4, 'Паспорт', '4008', 8),
            ('Екатерина', 'Екатеринина', '+70555666777', 5, 'Паспорт', '4009', 9),
            ('Анна', 'Аннова', '+70666888999', 5, 'Паспорт', '4010', 10)
            ON CONFLICT DO NOTHING;");
    }
}