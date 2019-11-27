using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using Domain.Entities;

namespace Domain.Concrete
{
    public class RegnumDBInitializer : DropCreateDatabaseAlways<RegNumDBContext>
    {
        protected override void Seed(RegNumDBContext context)
        {
            var roles = new List<Role>
                {
                    new Role {RoleID = 1, RoleName = "User"},
                    new Role {RoleID = 2, RoleName = "Admin"}
                    

                };

            roles.ForEach(x => context.Roles.Add(x));
            context.SaveChanges();



            var users = new List<User>
                {
                    new User {UserID = 1, RoleID = 2, Created = DateTime.Now, IsActivated = true, Email = "g43479@ya.ru", Phone = "89688138344", Login = "graf43479", Password = "FE608D7BDE2CB2B40D924C2DEA75541248BE79DA", PasswordSalt = "xdW7DAsL5fgit0ZbFPXDubAuVCJm+IAk0SEDrAnN+o0=",UserName = "Олег"},
                    new User {UserID = 2, RoleID = 1, Created = DateTime.Now, IsActivated = true, Email = "g43479@yandex.ru", Phone = "896000000", Login = "graf434791", Password = "6E6FDC3A675DE540BEBDC511D63EABF3164AA0DB", PasswordSalt = "pu8cV942Dran70eOiEBn7f7taWWgH6+qEAVbnaZP/mQ=",UserName = "Алех"}
                    //new User {UserID = 2, RoleID = 2, Created = DateTime.Now, IsActivated = true, Email = "graaf43479@ya.ru",Login = "graf434792", Password = "graf43479" },
                    //new User {UserID = 3, RoleID = 2, Created = DateTime.Now, IsActivated = true, Email = "graff43479@ya.ru",Login = "graf434793", Password = "graf43479" },
                    //new User {UserID = 4, RoleID = 1, Created = DateTime.Now, IsActivated = true, Email = "grafff43479@ya.ru",Login = "graf434794", Password = "graf43479" },
                    //new User {UserID = 5, RoleID = 1, Created = DateTime.Now, IsActivated = true, Email = "gra43479@ya.ru",Login = "graf434795", Password = "graf43479" }
                };
            users.ForEach(x => context.Users.Add(x));
            context.SaveChanges();

            var categories = new List<Category>
                {
                    //new Category {CategoryID = 1, CategoryName = "Золотые буквы", Description = "Все буквы номера одинаковые", ShortName = "cat1",CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 1},
                    //new Category {CategoryID = 2, CategoryName = "Золотые цифры", Description = "Все цифры в номере одинаковые", ShortName = "cat2", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 2},
                    //new Category {CategoryID = 3, CategoryName = "Региональный номер", Description = "Цифры номера совпадают с его регионом",ShortName = "cat3", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 3},
                    //new Category {CategoryID = 4, CategoryName = "Первая десятка", Description = "Первые десять номеров серии",ShortName = "cat4", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 4},
                    //new Category {CategoryID = 5, CategoryName = "Сотые", Description = "Номера вида \"*100**\",  \"*200**\" и т.д.",ShortName = "cat5", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 5},
                    //new Category {CategoryID = 6, CategoryName = "Зеркальные", Description = "Номера вида  \"*050**\",  \"*101**\" и т.д.",ShortName = "cat6", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 6},
                    //new Category {CategoryID = 7, CategoryName = "Спецномер", Description = "",ShortName = "cat7", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 7},
                    // new Category {CategoryID = 8, CategoryName = "Другое", Description = "",ShortName = "cat8", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 8}
                
                    new Category {CategoryID = 1, CategoryName = "Одинаковые буквы", Description = "Все буквы номера одинаковые", ShortName = "cat1",CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 1},
                    new Category {CategoryID = 2, CategoryName = "Одинаковые цифры", Description = "Все цифры в номере одинаковые", ShortName = "cat2", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 2},
                    new Category {CategoryID = 3, CategoryName = "Номер-регион", Description = "Цифры номера совпадают с его регионом",ShortName = "cat3", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 3},
                    new Category {CategoryID = 4, CategoryName = "Спецсерия", Description = "",ShortName = "cat4", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 4},
                     new Category {CategoryID = 5, CategoryName = "Эксклюзив", Description = "",ShortName = "cat5", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 5},
                       new Category {CategoryID = 6, CategoryName = "Другое", Description = "",ShortName = "cat6", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 6}
                };

            categories.ForEach(x => context.Categories.Add(x));
            context.SaveChanges();

            var regionTypes = new List<RegionType>
            {
                new RegionType {RegionTypeID = 1, RegionTypeDesc = "Неизвестно"},
                new RegionType {RegionTypeID = 1, RegionTypeDesc = "Москва"},
                new RegionType {RegionTypeID = 1, RegionTypeDesc = "Московская область"},
                new RegionType {RegionTypeID = 1, RegionTypeDesc = "Остальные регионы"}
            };


            regionTypes.ForEach(x => context.RegionTypes.Add(x));
            context.SaveChanges();

            var regions = new List<Region>
                {
                    new Region {RegionID=1, RegionNumber="80", RegionName = "Агинский Бурятский АО", IsActive=true, Sequence=3},
                    new Region {RegionID=2, RegionNumber="22", RegionName = "Алтайский край", IsActive=true, Sequence=3},
                    new Region {RegionID=3, RegionNumber="28", RegionName = "Амурская область", IsActive=true, Sequence=3},
                    new Region {RegionID=4, RegionNumber="29", RegionName = "Архангельская область", IsActive=true, Sequence=3},
                    new Region {RegionID=5, RegionNumber="30", RegionName = "Астраханская область", IsActive=true, Sequence=3},
                    new Region {RegionID=6, RegionNumber="31", RegionName = "Белгородская область", IsActive=true, Sequence=3},
                    new Region {RegionID=7, RegionNumber="32", RegionName = "Брянская область", IsActive=true, Sequence=3},
                    new Region {RegionID=8, RegionNumber="03", RegionName = "Бурятская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=9, RegionNumber="33", RegionName = "Владимирская область", IsActive=true, Sequence=3},
                    new Region {RegionID=10, RegionNumber="34", RegionName = "Волгоградская область", IsActive=true, Sequence=3},
                    new Region {RegionID=11, RegionNumber="35", RegionName = "Вологодская область", IsActive=true, Sequence=3},
                    new Region {RegionID=12, RegionNumber="36", RegionName = "Воронежская область", IsActive=true, Sequence=3},
                    new Region {RegionID=13, RegionNumber="97", RegionName = "г. Москва", IsActive=true, Sequence=1},
                    new Region {RegionID=14, RegionNumber="99", RegionName = "г. Москва", IsActive=true, Sequence=1},
                    new Region {RegionID=15, RegionNumber="177", RegionName = "г. Москва", IsActive=true, Sequence=1},
                    new Region {RegionID=16, RegionNumber="197", RegionName = "г. Москва", IsActive=true, Sequence=1},
                    new Region {RegionID=17, RegionNumber="199", RegionName = "г. Москва", IsActive=true, Sequence=1},
                    new Region {RegionID=18, RegionNumber="98", RegionName = "г. Санкт-Петербург", IsActive=true, Sequence=2},
                    new Region {RegionID=19, RegionNumber="77", RegionName = "г. Москва", IsActive=true, Sequence=1},
                    new Region {RegionID=20, RegionNumber="78", RegionName = "г. Санкт-Петербург", IsActive=true, Sequence=2},
                    new Region {RegionID=21, RegionNumber="79", RegionName = "Еврейская автономная область", IsActive=true, Sequence=3},
                    new Region {RegionID=22, RegionNumber="94", RegionName = "ЗАТО, г. Байконур", IsActive=true, Sequence=3},
                    new Region {RegionID=23, RegionNumber="37", RegionName = "Ивановская область", IsActive=true, Sequence=3},
                    new Region {RegionID=24, RegionNumber="06", RegionName = "Ингушская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=25, RegionNumber="38", RegionName = "Иркутская область", IsActive=true, Sequence=3},
                    new Region {RegionID=26, RegionNumber="138", RegionName = "Иркутская область", IsActive=true, Sequence=3},
                    new Region {RegionID=27, RegionNumber="07", RegionName = "Кабардино-Балкарская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=28, RegionNumber="39", RegionName = "Калининградская область", IsActive=true, Sequence=3},
                    new Region {RegionID=29, RegionNumber="40", RegionName = "Калужская область", IsActive=true, Sequence=3},
                    new Region {RegionID=30, RegionNumber="41", RegionName = "Камчатская область", IsActive=true, Sequence=3},
                    new Region {RegionID=31, RegionNumber="09", RegionName = "Карачаево-Черкесская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=32, RegionNumber="42", RegionName = "Кемеровская область", IsActive=true, Sequence=3},
                    new Region {RegionID=33, RegionNumber="43", RegionName = "Кировская область", IsActive=true, Sequence=3},
                    new Region {RegionID=34, RegionNumber="81", RegionName = "Коми-Пермяцкий АО", IsActive=true, Sequence=3},
                    new Region {RegionID=35, RegionNumber="82", RegionName = "Корякский автономный округ", IsActive=true, Sequence=3},
                    new Region {RegionID=36, RegionNumber="44", RegionName = "Костромская область", IsActive=true, Sequence=3},
                    new Region {RegionID=37, RegionNumber="23", RegionName = "Краснодарский край", IsActive=true, Sequence=3},
                    new Region {RegionID=38, RegionNumber="93", RegionName = "Краснодарский край", IsActive=true, Sequence=3},
                    new Region {RegionID=39, RegionNumber="24", RegionName = "Красноярский край", IsActive=true, Sequence=3},
                    new Region {RegionID=40, RegionNumber="45", RegionName = "Курганская область", IsActive=true, Sequence=3},
                    new Region {RegionID=41, RegionNumber="46", RegionName = "Курская область", IsActive=true, Sequence=3},
                    new Region {RegionID=42, RegionNumber="47", RegionName = "Ленинградская область", IsActive=true, Sequence=3},
                    new Region {RegionID=43, RegionNumber="48", RegionName = "Липецкая область", IsActive=true, Sequence=3},
                    new Region {RegionID=44, RegionNumber="49", RegionName = "Магаданская область", IsActive=true, Sequence=3},
                    new Region {RegionID=45, RegionNumber="13", RegionName = "Мордовская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=46, RegionNumber="50", RegionName = "Московская область", IsActive=true, Sequence=3},
                    new Region {RegionID=47, RegionNumber="90", RegionName = "Московская область", IsActive=true, Sequence=3},
                    new Region {RegionID=48, RegionNumber="150", RegionName = "Московская область", IsActive=true, Sequence=3},
                    new Region {RegionID=49, RegionNumber="51", RegionName = "Мурманская область", IsActive=true, Sequence=3},
                    new Region {RegionID=50, RegionNumber="83", RegionName = "Ненецкий автономный округ", IsActive=true, Sequence=3},
                    new Region {RegionID=51, RegionNumber="52", RegionName = "Нижегородская область", IsActive=true, Sequence=3},
                    new Region {RegionID=52, RegionNumber="152", RegionName = "Нижегородская область", IsActive=true, Sequence=3},
                    new Region {RegionID=53, RegionNumber="53", RegionName = "Новгородская область", IsActive=true, Sequence=3},
                    new Region {RegionID=54, RegionNumber="54", RegionName = "Новосибирская область", IsActive=true, Sequence=3},
                    new Region {RegionID=55, RegionNumber="154", RegionName = "Новосибирская область", IsActive=true, Sequence=3},
                    new Region {RegionID=56, RegionNumber="55", RegionName = "Омская область", IsActive=true, Sequence=3},
                    new Region {RegionID=57, RegionNumber="56", RegionName = "Оренбургская область", IsActive=true, Sequence=3},
                    new Region {RegionID=58, RegionNumber="57", RegionName = "Орловская область", IsActive=true, Sequence=3},
                    new Region {RegionID=59, RegionNumber="58", RegionName = "Пензенская область", IsActive=true, Sequence=3},
                    new Region {RegionID=60, RegionNumber="59", RegionName = "Пермская область", IsActive=true, Sequence=3},
                    new Region {RegionID=61, RegionNumber="159", RegionName = "Пермская область", IsActive=true, Sequence=3},
                    new Region {RegionID=62, RegionNumber="25", RegionName = "Приморский край", IsActive=true, Sequence=3},
                    new Region {RegionID=63, RegionNumber="125", RegionName = "Приморский край", IsActive=true, Sequence=3},
                    new Region {RegionID=64, RegionNumber="60", RegionName = "Псковская область", IsActive=true, Sequence=3},
                    new Region {RegionID=65, RegionNumber="01", RegionName = "Республика Адыгея", IsActive=true, Sequence=3},
                    new Region {RegionID=66, RegionNumber="04", RegionName = "Республика Алтай", IsActive=true, Sequence=3},
                    new Region {RegionID=67, RegionNumber="02", RegionName = "Республика Башкортостан", IsActive=true, Sequence=3},
                    new Region {RegionID=68, RegionNumber="102", RegionName = "Республика Башкортостан", IsActive=true, Sequence=3},
                    new Region {RegionID=69, RegionNumber="05", RegionName = "Республика Дагестан", IsActive=true, Sequence=3},
                    new Region {RegionID=70, RegionNumber="08", RegionName = "Республика Калмыкия", IsActive=true, Sequence=3},
                    new Region {RegionID=71, RegionNumber="10", RegionName = "Республика Карелия", IsActive=true, Sequence=3},
                    new Region {RegionID=72, RegionNumber="11", RegionName = "Республика Коми", IsActive=true, Sequence=3},
                    new Region {RegionID=73, RegionNumber="12", RegionName = "Республика Марий-Эл", IsActive=true, Sequence=3},
                    new Region {RegionID=74, RegionNumber="14", RegionName = "Республика Саха (Якутия)", IsActive=true, Sequence=3},
                    new Region {RegionID=75, RegionNumber="15", RegionName = "Республика Северная Осетия", IsActive=true, Sequence=3},
                    new Region {RegionID=76, RegionNumber="16", RegionName = "Республика Татарстан", IsActive=true, Sequence=3},
                    new Region {RegionID=77, RegionNumber="116", RegionName = "Республика Татарстан", IsActive=true, Sequence=3},
                    new Region {RegionID=78, RegionNumber="17", RegionName = "Республика Тува", IsActive=true, Sequence=3},
                    new Region {RegionID=79, RegionNumber="19", RegionName = "Республика Хакасия", IsActive=true, Sequence=3},
                    new Region {RegionID=80, RegionNumber="61", RegionName = "Ростовская область", IsActive=true, Sequence=3},
                    new Region {RegionID=81, RegionNumber="161", RegionName = "Ростовская область", IsActive=true, Sequence=3},
                    new Region {RegionID=82, RegionNumber="62", RegionName = "Рязанская область", IsActive=true, Sequence=3},
                    new Region {RegionID=83, RegionNumber="63", RegionName = "Самарская область", IsActive=true, Sequence=3},
                    new Region {RegionID=84, RegionNumber="163", RegionName = "Самарская область", IsActive=true, Sequence=3},
                    new Region {RegionID=85, RegionNumber="64", RegionName = "Саратовская область", IsActive=true, Sequence=3},
                    new Region {RegionID=86, RegionNumber="164", RegionName = "Саратовская область", IsActive=true, Sequence=3},
                    new Region {RegionID=87, RegionNumber="65", RegionName = "Сахалинская область", IsActive=true, Sequence=3},
                    new Region {RegionID=88, RegionNumber="66", RegionName = "Свердловская область", IsActive=true, Sequence=3},
                    new Region {RegionID=89, RegionNumber="96", RegionName = "Свердловская область", IsActive=true, Sequence=3},
                    new Region {RegionID=90, RegionNumber="67", RegionName = "Смоленская область", IsActive=true, Sequence=3},
                    new Region {RegionID=91, RegionNumber="26", RegionName = "Ставропольский край", IsActive=true, Sequence=3},
                    new Region {RegionID=92, RegionNumber="84", RegionName = "Таймырский автономный округ", IsActive=true, Sequence=3},
                    new Region {RegionID=93, RegionNumber="68", RegionName = "Тамбовская область", IsActive=true, Sequence=3},
                    new Region {RegionID=94, RegionNumber="69", RegionName = "Тверская область", IsActive=true, Sequence=3},
                    new Region {RegionID=95, RegionNumber="70", RegionName = "Томская область", IsActive=true, Sequence=3},
                    new Region {RegionID=96, RegionNumber="71", RegionName = "Тульская область", IsActive=true, Sequence=3},
                    new Region {RegionID=97, RegionNumber="72", RegionName = "Тюменская область", IsActive=true, Sequence=3},
                    new Region {RegionID=98, RegionNumber="18", RegionName = "Удмуртская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=99, RegionNumber="118", RegionName = "Удмуртская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=100, RegionNumber="73", RegionName = "Ульяновская область", IsActive=true, Sequence=3},
                    new Region {RegionID=101, RegionNumber="173", RegionName = "Ульяновская область", IsActive=true, Sequence=3},
                    new Region {RegionID=102, RegionNumber="85", RegionName = "Усть-Ордынский АО", IsActive=true, Sequence=3},
                    new Region {RegionID=103, RegionNumber="27", RegionName = "Хабаровский край", IsActive=true, Sequence=3},
                    new Region {RegionID=104, RegionNumber="86", RegionName = "Ханты-Мансийский АО", IsActive=true, Sequence=3},
                    new Region {RegionID=105, RegionNumber="74", RegionName = "Челябинская область", IsActive=true, Sequence=3},
                    new Region {RegionID=106, RegionNumber="174", RegionName = "Челябинская область", IsActive=true, Sequence=3},
                    new Region {RegionID=107, RegionNumber="95", RegionName = "Чеченская Республика (новые номера)", IsActive=true, Sequence=3},
                    new Region {RegionID=108, RegionNumber="20", RegionName = "Чеченская республика (старые номера)", IsActive=true, Sequence=3},
                    new Region {RegionID=109, RegionNumber="75", RegionName = "Читинская область", IsActive=true, Sequence=3},
                    new Region {RegionID=110, RegionNumber="21", RegionName = "Чувашская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=111, RegionNumber="121", RegionName = "Чувашская республика", IsActive=true, Sequence=3},
                    new Region {RegionID=112, RegionNumber="87", RegionName = "Чукотский автономный округ", IsActive=true, Sequence=3},
                    new Region {RegionID=113, RegionNumber="88", RegionName = "Эвенкийский автономный округ", IsActive=true, Sequence=3},
                    new Region {RegionID=114, RegionNumber="89", RegionName = "Ямало-Ненецкий АО", IsActive=true, Sequence=3},
                    new Region {RegionID=115, RegionNumber="76", RegionName = "Ярославская область", IsActive=true, Sequence=3}
                };

            regions.ForEach(x => context.Regions.Add(x));
            context.SaveChanges();


            var products = new List<Product>
                {
                new Product {ProductID = 1,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 10000, ProductNumber = "о056ММ78", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = true},
                new Product {ProductID = 2,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 15000, ProductNumber = "а545аа199", Status = "",UserID = 2, RegionID = 2, IsOverbalanceIncluded = true},
                new Product {ProductID = 3, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 20000, ProductNumber = "С89*СС77", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = true},
                new Product {ProductID = 4,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 25000, ProductNumber = "А444АА190", Status = "",UserID = 2, RegionID = 4, IsOverbalanceIncluded = true},
                new Product {ProductID = 5,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 30000, ProductNumber = "М667ММ50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = true},
                new Product {ProductID = 6,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 35000, ProductNumber = "О71*ОО190", Status = "",UserID = 2, RegionID = 6, IsOverbalanceIncluded = true},
                new Product {ProductID = 7,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 40000, ProductNumber = "М222мм50", Status = "",UserID = 1, RegionID = 7, IsOverbalanceIncluded = true},
                new Product {ProductID = 8,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 45000, ProductNumber = "М333мм50", Status = "",UserID = 2, RegionID = 8, IsOverbalanceIncluded = true},
                new Product {ProductID = 9,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 50000, ProductNumber = "М444мм190", Status = "",UserID = 1, RegionID = 9, IsOverbalanceIncluded = true},
                new Product {ProductID = 10, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 55000, ProductNumber = "а444аа190", Status = "",UserID = 2, RegionID = 1, IsOverbalanceIncluded = true},
                new Product {ProductID = 11, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 60000, ProductNumber = "а333аа190", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = true},
                new Product {ProductID = 12, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 65000, ProductNumber = "а222аа190", Status = "",UserID = 2, RegionID = 3, IsOverbalanceIncluded = true},
                new Product {ProductID = 13, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 70000, ProductNumber = "а111аа190", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = true},
                new Product {ProductID = 14, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 75000, ProductNumber = "а555аа50", Status = "",UserID = 2, RegionID = 5, IsOverbalanceIncluded = true},
                new Product {ProductID = 15, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 80000, ProductNumber = "а666аа50", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = true},
                new Product {ProductID = 16, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 85000, ProductNumber = "а777аа50", Status = "",UserID = 2, RegionID = 2, IsOverbalanceIncluded = true},
                new Product {ProductID = 17, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 90000, ProductNumber = "а888аа77", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = true},
                new Product {ProductID = 18, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 95000, ProductNumber = "а999аа50", Status = "",UserID = 2, RegionID = 4, IsOverbalanceIncluded = true},
                new Product {ProductID = 19,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 100000, ProductNumber = "а101аа190", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = true},
                new Product {ProductID = 20,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 105000, ProductNumber = "а252аа50", Status = "",UserID = 2, RegionID = 1, IsOverbalanceIncluded = true},
                new Product {ProductID = 21,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 110000, ProductNumber = "а101аа50", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = true},
                new Product {ProductID = 22,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 115000, ProductNumber = "а202аа777", Status = "",UserID = 2, RegionID = 3, IsOverbalanceIncluded = true},
                new Product {ProductID = 23, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 120000, ProductNumber = "а303аа50", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 24,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 125000, ProductNumber = "а404аа50", Status = "",UserID = 2, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 25,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 130000, ProductNumber = "а505аа77", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 26,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 135000, ProductNumber = "а606аа50", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = false},
                new Product {ProductID = 27,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 140000, ProductNumber = "а707аа50", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = false},
                new Product {ProductID = 28,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 11000, ProductNumber = "а808аа77", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = false},
                new Product {ProductID = 29,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 12000, ProductNumber = "к500кк98", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 30,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 13000, ProductNumber = "к100кк190", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 31, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 14000, ProductNumber = "к200кк98", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = false},
                new Product {ProductID = 32, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 15000, ProductNumber = "к300кк90", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = false},
                new Product {ProductID = 33,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 16000, ProductNumber = "к400кк190", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = false},
                new Product {ProductID = 34, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 17000, ProductNumber = "к500кк50", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 35,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 18000, ProductNumber = "к600кк178", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 36,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 19000, ProductNumber = "к700кк98", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = false},
                new Product {ProductID = 37, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 20000, ProductNumber = "к800кк98", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = false},
                new Product {ProductID = 38,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 21000, ProductNumber = "е444ее98", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = false},
                new Product {ProductID = 39, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 22000, ProductNumber = "е111ее98", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 40,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 23000, ProductNumber = "е222ее98", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 41,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 24000, ProductNumber = "е333ее98", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 42,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 25000, ProductNumber = "е444ее190", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 43,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 26000, ProductNumber = "е555ее98", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = false},
                new Product {ProductID = 44,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 27000, ProductNumber = "е666ее98", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = false},
                new Product {ProductID = 45,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 28000, ProductNumber = "е777ее98", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = false},
                new Product {ProductID = 46,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 29000, ProductNumber = "е888ее50", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 47,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 30000, ProductNumber = "е999ее178", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 48, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 31000, ProductNumber = "а804аа190", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = false},
                new Product {ProductID = 49,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 32000, ProductNumber = "а805аа50", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = false},
                new Product {ProductID = 50,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 33000, ProductNumber = "а806аа50", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = false},
                new Product {ProductID = 51,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 34000, ProductNumber = "а807аа50", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 52,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 35000, ProductNumber = "а808аа50", Status = "",UserID = 1, RegionID = 1, IsOverbalanceIncluded = false},
                new Product {ProductID = 53,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 36000, ProductNumber = "а809аа190", Status = "",UserID = 1, RegionID = 2, IsOverbalanceIncluded = false},
                new Product {ProductID = 54, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 37000, ProductNumber = "а810аа190", Status = "",UserID = 1, RegionID = 3, IsOverbalanceIncluded = false},
                new Product {ProductID = 55,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 38000, ProductNumber = "а811аа178", Status = "",UserID = 1, RegionID = 4, IsOverbalanceIncluded = false},
                new Product {ProductID = 56,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а813аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 57,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а812аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 58,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а814аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 59,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а815аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 60, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а816аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 61, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а817аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 62,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а818аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 63,  StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а819аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false},
                new Product {ProductID = 64, StartDate = DateTime.Now, UpdateDate = DateTime.Now, IsForSale = true, IsChoosen = false, IsDisplay = true, Price = 39000, ProductNumber = "а822аа50", Status = "",UserID = 1, RegionID = 5, IsOverbalanceIncluded = false}
                };

            products.ForEach(x => context.Products.Add(x));
            context.SaveChanges();


            context.Configuration.ValidateOnSaveEnabled = false;
            var comments = new List<Comment>
                {
                    new Comment{CommentID = 1, Tittle = "Мария , школа моделей Grace Models School", CreateDate = DateTime.Now, IsAccept = true,  AnswerText  = "Cum dicit mollis accusam no, iudico integre principes ex eos, pro habemus salutandi ei. Harum rationibus mei ea, debet singulis vis ea, errem pertinax corrumpit mei ea. Ei eam case accumsan mnesarchum, nonumy numquam scribentur nam an. Vel amet consetetur dissentiunt ea, ad nec recteque dissentiet, ne eum latine disputando. Vix cu mundi nihil feugiat, ex nulla gubergren interpretaris cum.", QuestionText = "Lorem ipsum dolor sit amet, timeam necessitatibus pri ex, posse meliore salutatus id eam?"},
                    new Comment{CommentID = 2, Tittle = "Александр и Наталья", CreateDate = DateTime.Now, IsAccept = true, AnswerText  = "Cum dicit mollis accusam no, iudico integre principes ex eos, pro habemus salutandi ei. Harum rationibus mei ea, debet singulis vis ea, errem pertinax corrumpit mei ea. Ei eam case accumsan mnesarchum, nonumy numquam scribentur nam an. Vel amet consetetur dissentiunt ea, ad nec recteque dissentiet, ne eum latine disputando. Vix cu mundi nihil feugiat, ex nulla gubergren interpretaris cum.",QuestionText = "Sed ut aeque essent, nam saperet nominavi ne, vix possit vocent placerat ad. Ius eu malorum feugiat comprehensam?"},
                    new Comment{CommentID = 3, Tittle = "Алина", CreateDate = DateTime.Now, IsAccept = true,  AnswerText  = "Lorem ipsum dolor sit amet, timeam necessitatibus pri ex, posse meliore salutatus id eam. Sed ut aeque essent, nam saperet nominavi ne, vix possit vocent placerat ad. Ius eu malorum feugiat comprehensam. Quo te partem maiorum maluisset. Ut laudem melius perpetua sed, verear dolores vel et.Cum ferri consul urbanitas ad, vim possit dicunt meliore ex. No pri etiam conclusionemque. Vis fuisset rationibus consectetuer ex. Pri eu propriae delicata vulputate, putant nonumes mea te, cu tempor cetero honestatis vis. Nec scripta persequeris cu, tollit laoreet concludaturque vis no.Dolore mediocrem nam et. Atqui audiam facilis usu cu, et eos vide luptatum pertinacia. Eu has feugiat molestie, eu vis esse inani constituto. Agam dicta eum et, vim at probo erroribus, sea esse sonet detraxit id. Id eam verear neglegentur, nec ne decore perpetua dissentiet.Ne cum velit semper", QuestionText = "Quo te partem maiorum maluisset. Ut laudem melius perpetua sed, verear dolores vel et?"},
                    new Comment{CommentID = 4, Tittle = "Карасевы", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "Elit harum oporteat id vis. Nullam nonumes ne vim, justo voluptatum nam no. Ex per adhuc eripuit persequeris, eu eam porro voluptatum, mei stet soluta eu. Ea sint agam comprehensam has.Cum dicit mollis accusam no, iudico integre principes ex eos, pro habemus salutandi ei. Harum rationibus mei ea, debet singulis vis ea, errem pertinax corrumpit mei ea. Ei eam case accumsan mnesarchum, nonumy numquam scribentur nam an. Vel amet consetetur dissentiunt ea, ad nec recteque dissentiet, ne eum latine disputando. Vix cu mundi nihil feugiat, ex nulla gubergren interpretaris cum.", QuestionText = "Cum ferri consul urbanitas ad, vim possit dicunt meliore ex. No pri etiam conclusionemque?"},
                    new Comment{CommentID = 5, Tittle = "Анастасия", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "Lorem ipsum dolor sit amet, timeam necessitatibus pri ex, posse meliore salutatus id eam. Sed ut aeque essent, nam saperet nominavi ne, vix possit vocent placerat ad. Ius eu malorum feugiat comprehensam. Quo te partem maiorum maluisset. Ut laudem melius perpetua sed, verear dolores vel et", QuestionText = "Vis fuisset rationibus consectetuer ex?"},
                    new Comment{CommentID = 6, Tittle = "Дарья", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "test", QuestionText = "Pri eu propriae delicata vulputate, putant nonumes mea te, cu tempor cetero honestatis vis?"},
                    new Comment{CommentID = 7, Tittle = "Елена", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "test", QuestionText = "Nec scripta persequeris cu, tollit laoreet concludaturque vis no.Dolore mediocrem nam et. Atqui audiam facilis usu cu, et eos vide luptatum pertinacia. Eu has feugiat molestie, eu vis esse inani constituto. Agam dicta eum et, vim at probo erroribus, sea esse sonet detraxit id?"},
                    new Comment{CommentID = 8, Tittle = "Саша и Сережа", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "test", QuestionText = " Id eam verear neglegentur, nec ne decore perpetua dissentiet.Ne cum velit semper. Elit harum oporteat id vis?"},
                    new Comment{CommentID = 9, Tittle = "Мария", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "test", QuestionText = "Nullam nonumes ne vim, justo voluptatum nam no?"},
                    new Comment{CommentID = 10, Tittle = "Евгений", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "test", QuestionText = "Ex per adhuc eripuit persequeris, eu eam porro voluptatum, mei stet soluta eu?"},
                    new Comment{CommentID = 11, Tittle = "Наталья", CreateDate = DateTime.Now, IsAccept = true, AnswerDate =DateTime.Now, AnswerText  = "test", QuestionText = "Ea sint agam comprehensam has?"},
                };
            comments.ForEach(x => context.Comments.Add(x));
            context.SaveChanges();
            context.Configuration.ValidateOnSaveEnabled = true;
            //using (context)
           // {
              //  context.Configuration.ValidateOnSaveEnabled = false;

                var articles = new List<Article>
                {
                    //new Article{ArticleID = 1, CreateDate = DateTime.Now, UserID = 1, Snippet = "О нас",  ArticleText = "<h1>О нас</h1><div class='page-content'><p><a class='modal' href='upload-files/i.jpg'><img style='margin-left: 10px; margin-right: 10px; float: right;' src='@Url.Content('~/Content/images/i.jpg')' alt='' width='200' height='301' /></a>Я думаю, что человек лучше всего делает то, к чему испытывает неподдельную страсть. Вот почему я занимаюсь фотографией. Во время фотосъемки для меня важно передать атмосферу и настоящие эмоции. Акцентирую внимание на ярких деталях и стараюсь добавить в каждый кадр немного динамики. Самое важное в фотографии для меня &ndash; раскрыть характер человека, его внутренний мир, поймать правильный момент. И результат этой работы &ndash; яркие, динамичные и красивые снимки, которые, надеюсь, радуют и очаровывают людей.</p><p>Есть журнальные публикации, иллюстраций к статьям на Интернет-ресурсах, мои фотографии использовались в рекламе.</p><p>Постоянно повышаю квалификацию на различных курсах и мастер-классах.</p></div>"}
                    new Article{ArticleID = 1, CreateDate = DateTime.Now, UserID = 1, Snippet = "О нас", Header = "test", ArticlePreview = "test", ShortLink = "article-1", Keywords = "test", ArticleText = "<h1>О нас</h1><div class='page-content'><p><a class='modal' href='upload-files/i.jpg'><img style='margin-left: 10px; margin-right: 10px; float: right;' src='../Content/images/i.jpg' alt='' width='200' height='301' /></a>Я думаю, что человек лучше всего делает то, к чему испытывает неподдельную страсть. Вот почему я занимаюсь фотографией. Во время фотосъемки для меня важно передать атмосферу и настоящие эмоции. Акцентирую внимание на ярких деталях и стараюсь добавить в каждый кадр немного динамики. Самое важное в фотографии для меня &ndash; раскрыть характер человека, его внутренний мир, поймать правильный момент. И результат этой работы &ndash; яркие, динамичные и красивые снимки, которые, надеюсь, радуют и очаровывают людей.</p><p>Есть журнальные публикации, иллюстраций к статьям на Интернет-ресурсах, мои фотографии использовались в рекламе.</p><p>Постоянно повышаю квалификацию на различных курсах и мастер-классах.</p></div>"}
                };
                articles.ForEach(x => context.Articles.Add(x));

                context.SaveChanges();
            //}



                var seo = new List<SeoAttribute>
                {
                    new SeoAttribute() {TagID = "About", Keywords = "о нас", Snippet = "test", Robots = "follow, index", Tittle = "О нас",  UserID = 1, CreateDate = DateTime.Now.AddDays(-15), UpdateDate = DateTime.Now.AddDays(-27), Header = "test", ArticlePreview = "test", ShortLink = "article-1", ArticleText = "<h1>О нас</h1><div class='page-content'><p><a class='modal' href='upload-files/i.jpg'><img style='margin-left: 10px; margin-right: 10px; float: right;' src='../Content/images/i.jpg' alt='' width='200' height='301' /></a>Я думаю, что человек лучше всего делает то, к чему испытывает неподдельную страсть. Вот почему я занимаюсь фотографией. Во время фотосъемки для меня важно передать атмосферу и настоящие эмоции. Акцентирую внимание на ярких деталях и стараюсь добавить в каждый кадр немного динамики. Самое важное в фотографии для меня &ndash; раскрыть характер человека, его внутренний мир, поймать правильный момент. И результат этой работы &ndash; яркие, динамичные и красивые снимки, которые, надеюсь, радуют и очаровывают людей.</p><p>Есть журнальные публикации, иллюстраций к статьям на Интернет-ресурсах, мои фотографии использовались в рекламе.</p><p>Постоянно повышаю квалификацию на различных курсах и мастер-классах.</p></div>"},
                    new SeoAttribute() {TagID = "Portfolio", Keywords = "База номеров", Snippet = "Сниппет База номеров", Robots = "follow, index", Tittle = "База номеров", UserID = 1, CreateDate = DateTime.Now.AddDays(-35), UpdateDate = DateTime.Now.AddDays(-5)},
                    new SeoAttribute() {TagID=  "Contact", Keywords = "Контакты", Snippet = "Сниппет Контакты", Robots = "follow, index", Tittle = "Контакты", UserID = 1, CreateDate = DateTime.Now.AddDays(-15), UpdateDate = DateTime.Now.AddDays(-20)}
                    
                };
                seo.ForEach(x => context.SeoAttributes.Add(x));
                context.SaveChanges();

                //var seoarticle = new List<SeoArticleBase>
                //{
                //    new SeoArticleBase() {TagID = "About", CreateDate = DateTime.Now, UpdateDate = DateTime.Now},
                //    new SeoArticleBase() { TagID = "Portfolio",CreateDate = DateTime.Now, UpdateDate = DateTime.Now},
                //    new SeoArticleBase() { TagID = "Contact", CreateDate = DateTime.Now, UpdateDate = DateTime.Now}
                //};
                //seoarticle.ForEach(x => context.SeoArticleBases.Add(x));
                //context.SaveChanges();

         //       var seo2 = new List<Seo>
         //       {
         //           new Seo() {TagID = "About", Keywords = "о нас", Snippet = "test", Robots = "follow, index", Tittle = "О нас", CreateDate = DateTime.Now, UpdateDate = DateTime.Now},
         //           new Seo() { TagID = "Portfolio", Keywords = "База номеров", Snippet = "Сниппет База номеров", Robots = "follow, index", Tittle = "База номеров", CreateDate = DateTime.Now, UpdateDate = DateTime.Now},
         //           new Seo() { TagID = "Contact", Keywords = "Контакты", Snippet = "Сниппет Контакты", Robots = "follow, index", Tittle = "Контакты", CreateDate = DateTime.Now, UpdateDate = DateTime.Now}
                    
         //       };
         //       seo2.ForEach(x => context.Seos.Add(x));
         //     //  context.SaveChanges();  

         ////   Thread.Sleep(1000);
         //       var articlesDerived = new List<ArticleDerived>
         //       {
         //           //new Article{ArticleID = 1, CreateDate = DateTime.Now, UserID = 1, Snippet = "О нас",  ArticleText = "<h1>О нас</h1><div class='page-content'><p><a class='modal' href='upload-files/i.jpg'><img style='margin-left: 10px; margin-right: 10px; float: right;' src='@Url.Content('~/Content/images/i.jpg')' alt='' width='200' height='301' /></a>Я думаю, что человек лучше всего делает то, к чему испытывает неподдельную страсть. Вот почему я занимаюсь фотографией. Во время фотосъемки для меня важно передать атмосферу и настоящие эмоции. Акцентирую внимание на ярких деталях и стараюсь добавить в каждый кадр немного динамики. Самое важное в фотографии для меня &ndash; раскрыть характер человека, его внутренний мир, поймать правильный момент. И результат этой работы &ndash; яркие, динамичные и красивые снимки, которые, надеюсь, радуют и очаровывают людей.</p><p>Есть журнальные публикации, иллюстраций к статьям на Интернет-ресурсах, мои фотографии использовались в рекламе.</p><p>Постоянно повышаю квалификацию на различных курсах и мастер-классах.</p></div>"}
         //           new ArticleDerived(){TagID = "About", UserID = 1, Snippet = "О нас", Header = "test", ArticlePreview = "test", ShortLink = "article-1", Keywords = "test", ArticleText = "<h1>О нас</h1><div class='page-content'><p><a class='modal' href='upload-files/i.jpg'><img style='margin-left: 10px; margin-right: 10px; float: right;' src='../Content/images/i.jpg' alt='' width='200' height='301' /></a>Я думаю, что человек лучше всего делает то, к чему испытывает неподдельную страсть. Вот почему я занимаюсь фотографией. Во время фотосъемки для меня важно передать атмосферу и настоящие эмоции. Акцентирую внимание на ярких деталях и стараюсь добавить в каждый кадр немного динамики. Самое важное в фотографии для меня &ndash; раскрыть характер человека, его внутренний мир, поймать правильный момент. И результат этой работы &ndash; яркие, динамичные и красивые снимки, которые, надеюсь, радуют и очаровывают людей.</p><p>Есть журнальные публикации, иллюстраций к статьям на Интернет-ресурсах, мои фотографии использовались в рекламе.</p><p>Постоянно повышаю квалификацию на различных курсах и мастер-классах.</p></div>"}
         //       };
            


//            foreach (var a in articlesDerived)
//            {
//                //var s = context.SeoArticleBases.FirstOrDefault(x => x.TagID == a.TagID);
                
//                   // context.Entry(s).State = EntityState.Detached;
//                   // context.Entry(a).State = EntityState.Modified;
//                SeoArticleBase sb = context.SeoArticleBases.SingleOrDefault<SeoArticleBase>(b => b.TagID == a.TagID);
//                if (sb!=null)
//                {
//                    if (sb.GetType()==typeof(ArticleDerived))
//                    {
//                        ((ArticleDerived)sb).ArticlePreview = a.ArticlePreview;
//                        ((ArticleDerived)sb).ShortLink= a.ShortLink;

//                    }
//                    context.SaveChanges();
//                }
//                //context.ArticleDeriveds.Add(a);
                   
                            
                            
                      

//                   //context.SaveChanges();
                

//                //context.ArticleDeriveds.Local.Add(a);
                
//}
                //SeoArticleBase base1 = context.SeoArticleBases.FirstOrDefault(x => x.TagID == a.TagID);
                //if (base1!=null)
               // {
                    //context.Entry(base1).State = EntityState.Modified;
                
                    //context.ArticleDeriveds.Add(new ArticleDerived()
                    //{
                    //    ArticlePreview = a.ArticlePreview,
                    //    ArticleText = a.ArticleText,
                    //    Header = a.Header,
                    //    ShortLink = a.ShortLink,
                    //    Snippet = a.Snippet,
                    //    UserID = a.UserID
                    //});
                
            //        context.SaveChanges();    
               
                    //context.Entry(p).State = EntityState.Detached;

//                    context.Entry(a).State = EntityState.Modified;
                
                

                    //context.Entry(a).State = EntityState.Added;
                    
                  //  context.SaveChanges();
                //}
            


            //articlesDerived.ForEach(x => context.ArticleDeriveds.Attach(x));
                
                



        
               
            //articlesDerived.ForEach(x => context.Entry(x).State=EntityState.Modified);
            
                //context.Entry(articlesDerived).State = EntityState.Modified;
               // context.SaveChanges();

                var mailSattings = new List<MailSettings>
                {
                    new MailSettings() {MailSettingsID = "MAIL_TO_ADDRESS", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Email адресата", SettingsValue = Constants.MAIL_TO_ADDRESS},
                    new MailSettings() {MailSettingsID = "MAIL_FROM_ADDRESS", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Email сервера отправки", SettingsValue = Constants.MAIL_FROM_ADDRESS},
                    new MailSettings() {MailSettingsID = "MAIL_USE_SSL", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Использовать SSL", SettingsValue = Constants.USE_SSL.ToString()},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_USER_NAME", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Логин почты", SettingsValue = Constants.USERNAME},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_PASSWORD", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Пароль", SettingsValue = Constants.PASSWORD},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_NAME", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Имя сервера", SettingsValue = Constants.SERVERNAME},
                    new MailSettings() {MailSettingsID = "MAIL_WRITE_AS_FILE", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Записывать как файл", SettingsValue = Constants.WRITE_AS_FILE.ToString()},
                    new MailSettings() {MailSettingsID = "MAIL_FILE_LOCATION", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Местонахождение файла", SettingsValue = Constants.FILE_LOCATION},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_PORT", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Порт", SettingsValue = Constants.SERVER_PORT.ToString()},
                    
                };
                mailSattings.ForEach(x => context.MailSettingses.Add(x));
                context.SaveChanges();

              

        }

        



        public static int DigitGnerate()
        {
            Random rnd = new Random();
            return rnd.Next(0, 20);
        }

    }

}