using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace RR.Data.DataBaseObjects;

[Table(nameof(ItemDBO))]
public class ItemDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public ICollection<ItemCategoryDBO> ItemCategories { get; set; }
}


public class CategoryDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public ICollection<ItemCategoryDBO> ItemCategories { get; set; }
    public ICollection<CategoryPropertyValueDBO> CategoryPropertyValues { get; set; }
}

[Table("CategoryProperty")]
public class CategoryPropertyDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PropertyId { get; set; }
    public string PropertyName { get; set; }
    public bool IsEnum { get; set; }
    public ICollection<PropertyEnumValueDBO> EnumValues { get; set; }
}

[Table("PropertyEnumValue")]
public class PropertyEnumValueDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EnumValueId { get; set; }
    public int PropertyId { get; set; }
    public string EnumValue { get; set; }
    [ForeignKey("PropertyId")]
    public CategoryPropertyDBO CategoryProperty { get; set; }
    public ICollection<CategoryPropertyValueDBO> CategoryPropertyValues { get; set; }
}

[Table("ItemCategory")]
public class ItemCategoryDBO
{
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey("ReceiptItemDBOId")]
    public ReceiptItemDBO Item { get; set; }
    [ForeignKey("CategoryId")]
    public CategoryDBO Category { get; set; }
}

[Table("CategoryPropertyValue")]
public class CategoryPropertyValueDBO
{
    public int CategoryId { get; set; }
    public int EnumValueId { get; set; }
    [ForeignKey("CategoryId")]
    public CategoryDBO Category { get; set; }
    [ForeignKey("EnumValueId")]
    public PropertyEnumValueDBO PropertyEnumValue { get; set; }
}
