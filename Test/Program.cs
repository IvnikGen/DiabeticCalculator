using SqlConnector.Methods;
using SqlConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //sqlRead();

            //List<PersonalArea> persarea = Read.getAreaTable();

            Console.ReadKey();
        }

        static void sqlRead()
        {
            List<ProductGroups> listPG = Read.getProductGroups();
            List<Products> listP = Read.getBreadUnitsTable();

            if (listPG == null)
                Console.WriteLine("Exception: " + Read.exception);
            else
            {
                foreach (var item in listP)
                {
                    item.ProductGroupName = listPG.First(x => x.GroupID == item.ProductGroup).GroupName;

                    Console.WriteLine(item.ID + "\t" + item.Product + "\t" + item.ProductGroup + "\t" + item.ProductGroupName);
                }
            }

        }

        static void sqlInsert()
        {
            //bool res = Create.insertProductGroups(new ProductGroups
            //{
            //    GroupName = "Test Group",
            //    GroupImage = "Test.image"
            //});

            //Console.WriteLine("RESULT: " + res);
            //sqlRead();

            bool res = Create.insertProduct(new Products
            {
                Product = "test product",
                ProductGroup = 1,
                Carbohydrates = 2.4f,
                GrammInUnit = 100,
                BreadUnits = 0
            });

            Console.WriteLine("RESULT: " + res);
            sqlRead();
        }

        static void sqlUpdate()
        {
            //bool res = Update.updateProductGroups(new ProductGroups
            //{
            //    GroupID = 0,
            //    GroupName = "Test Group 2",
            //    GroupImage = "Test.image2"
            //});

            bool res = Update.updateBreadUnitsTable(new Products
            {
                ID = 0,
                Product = "test product 2",
                ProductGroup = 2,
                Carbohydrates = 2.4f,
                GrammInUnit = 100,
                BreadUnits = 0
            });
            Console.WriteLine("RESULT: " + res);
            sqlRead();
        }

        static void sqlDelete()
        {
            //bool res = Delete.deleteProductGroups(new ProductGroups
            //{
            //    GroupID = 0,
            //    GroupName = "Test Group 2",
            //    GroupImage = "Test.image2"
            //});

            bool res = Delete.deleteBreadUnitsTable(new Products
            {
                ID = 0,
                Product = "test product 2",
                ProductGroup = 2,
                Carbohydrates = 2.4f,
                GrammInUnit = 100,
                BreadUnits = 0
            });
            Console.WriteLine("RESULT: " + res);
            sqlRead();
        }
    }
}
