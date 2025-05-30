//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace k1stroy.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.Orders = new HashSet<Orders>();
        }
    
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int ManufacturerID { get; set; }
        public int CategoryID { get; set; }
        public int TypeID { get; set; }
        public string Article { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
        public string ProductPhotoName { get; set; }
        public byte[] Photo { get; set; }
    
        public virtual Manufacturers Manufacturers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}
