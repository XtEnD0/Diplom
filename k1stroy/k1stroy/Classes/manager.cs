using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace k1stroy.Classes
{
    public class manager
    {
        public static int Leave;
        public static Data.Users CurrentUser { get; set; }
        public static Frame MainFrame { get; set; }

        //public static void GetImageData()
        //{
        //    var list = Data.k1stroyDBEntities.GetContext().Products.ToList();
        //    foreach (var item in list)
        //    {
        //        string path = Directory.GetCurrentDirectory() + @"\img\" + item.ProductPhotoName;
        //        if (File.Exists(path))
        //        {
        //            item.Photo = File.ReadAllBytes(path);
        //        }
        //        Data.k1stroyDBEntities.GetContext().SaveChanges();
        //    }
        //}

    }
}
