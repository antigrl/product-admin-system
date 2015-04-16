using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PAS.Models
{
    public class EnumNPREntities
    {
    }

    // Fee Enums
    #region FeeEnums
    public enum FeeConnectionList
    {
        [Description("Company Fee")]
        Company,
        [Description("Campaign Fee")]
        Campaign,
        [Description("Pricing Tier Fee")]
        Pricing_Tier,
        [Description("Product Fee")]
        Product,
        [Description("Sell Price Fee")]
        SellPrice,
        Null
    }
    public enum FeeTypeList
    {
        [Description("Amortized")]
        Amortized,
        [Description("Dollar Amount")]
        Dollar_Amount,
        [Description("Percent")]
        Percent
    }
    public enum PercentTypeList
    {
        [Description("Multiplication")]
        Multiplication,
        [Description("Division")]
        Division
    }
    public enum AmortizedTypeList
    {
        [Description("Vendor Minimum")]
        Vendor_Minimum,
        [Description("Initial Order Quantity")]
        Initial_Order_Quantity,
        [Description("Annual Sales Projection")]
        Annual_Sales_Projection,
        [Description("GatewayCDI Minimum Order")]
        GatewayCDI_Minimum_Order
    }
    public enum FeeCalcuationList
    {
        [Description("To Item Cost")]
        To_Item_Cost,
        [Description("To Item Net Cost")]
        To_Item_Net_Cost,
        [Description("To Item Total Cost")]
        To_Item_Total_Cost,
        [Description("To Margin")]
        To_Margin,
        [Description("To Sell Price")]
        To_Sell_Price,
        [Description("New Product Line")]
        New_Product_Line,
        Null
    }
    #endregion

    // Product Enums
    #region ProductEnums
    public enum ProductUpchargeNames
    {
        [Description("XL")]
        XL,
        [Description("2XL")]
        XXL,
        [Description("3XL")]
        XXXL,
        [Description("4XL")]
        XXXXL,
        [Description("5XL")]
        XXXXXL,
        [Description("6XL")]
        XXXXXXL,
        [Description("LT")]
        LT,
        [Description("XLT")]
        XLT,
        [Description("2XLT")]
        XXLT,
        [Description("3XLT")]
        XXXLT
    }

    public enum CategoryTypeList
    {
        [Description("Major")]
        MAJOR,
        [Description("Minor")]
        MINOR
    }

    #region SizeEnums
    public enum AdultSizes
    {
        [Description("XS")]
        XS,
        [Description("S")]
        S,
        [Description("M")]
        M,
        [Description("L")]
        L,
        [Description("XL")]
        XL,
        [Description("2XL")]
        XXL,
        [Description("3XL")]
        XXXL,
        [Description("4XL")]
        XXXXL,
        [Description("LT")]
        LT,
        [Description("XLT")]
        XLT
    }
    public enum YouthSizes
    {
        [Description("XS")]
        XS,
        [Description("S")]
        S,
        [Description("M")]
        M,
        [Description("L")]
        L,
        [Description("XL")]
        XL
    }
    public enum ToddlerSizes
    {
        [Description("2T")]
        Two_T,
        [Description("3T")]
        Three_T,
        [Description("4T")]
        Four_T,
        [Description("5T")]
        Five_T,
        [Description("6T")]
        Six_T,
        [Description("5/6")]
        Five_Six
    }
    public enum InfantSizes
    {
        [Description("NB")]
        NB,
        [Description("3M")]
        Three_M,
        [Description("6M")]
        Six_M,
        [Description("12M")]
        Twelve_M,
        [Description("18M")]
        Eightteen_M,
        [Description("24M")]
        TwentyFour_M
    }
    #endregion
    #endregion

    // Other Enums
    public enum EmailTo
    {
        [Description("Account Manager")]
        Account_Manager,
        [Description("Inventory Buyers")]
        Inventory_Buyers,
        [Description("Merchandisers")]
        Merchandisers,
        [Description("Mentor")]
        Mentor,
        Null
    }

    public enum Status
    {
        [Description("New")]
        New,
        [Description("Send to Merchandisers")]
        Send_To_Merchandisers,
        [Description("Send to Inventory Buyers")]
        Send_To_Inventory_Buyers,
        [Description("Send to Account Manager")]
        Send_To_Account_Manager,
        [Description("Send to Mentor")]
        Send_To_Mentor,
        [Description("Mentor Approved")]
        Mentor_Approved,
        [Description("Active")]
        Active,
        [Description("Rejected")]
        Rejected,
        [Description("Discontinued")]
        Discontinued,
        [Description("Archived")]
        Archived
    }

    public enum ObjectType
    {
        [Description("Campaign")]
        Campaign,
        [Description("Category")]
        Category,
        [Description("Company")]
        Company,
        [Description("Decoration Method")]
        Decoration_Method,
        [Description("Fee")]
        Fee,
        [Description("FeeName")]
        Fee_Name,
        [Description("Packaging Type")]
        Packaging_Type,
        [Description("Pricing Tier")]
        Pricing_Tier,
        [Description("Product")]
        Product,
        [Description("Product Decoration")]
        Product_Decoration,
        [Description("Product Sell Price")]
        Product_Sell_Price,
        [Description("Product Upcharge")]
        Product_Upcharge,
        [Description("Upcharge Sell Price")]
        Upcharge_Sell_Price,
        [Description("Vendor Name")]
        Vendor_Name,
        [Description("Vendor Type")]
        Vendor_Type,
        [Description("Major Category Ordering")]
        Major_Category_Ordering,
        Null
    }
}