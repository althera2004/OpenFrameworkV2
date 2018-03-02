// --------------------------------
// <copyright file="Rules.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core
{
    /// <summary>List of form validation rules</summary>
    public enum ValidatorRule
    {
        /// <summary>Undefined validation</summary>
        Undefined = 0,

        /// <summary>Minimum value</summary>        
        MinValue = 1,

        /// <summary>Maximum value</summary>   
        MaxValue = 2,

        /// <summary>Range value</summary>   
        RangeValue = 3,

        /// <summary>Minimum date</summary>   
        MinDate = 4,

        /// <summary>Maximum date</summary>   
        MaxDate = 5,

        /// <summary>Range of dates</summary>   
        RangeDate = 6,

        /// <summary>Today is the minimum value</summary>   
        MinToday = 7,

        /// <summary>Today is the maximum date</summary>   
        MaxToday = 8,

        /// <summary>Minimum options selected</summary>   
        MinSelected = 9,

        /// <summary>Maximum options selected</summary>   
        MaxSelected = 10,

        /// <summary>Only numbers</summary>   
        OnlyNumbers = 11,

        /// <summary>Data should be an email</summary>   
        Email = 12,

        /// <summary>Minimum length value</summary>   
        MinLength = 13,

        /// <summary>Maximum length value</summary>   
        MaxLength = 14,

        /// <summary>Date minor than another date field value</summary>   
        DateLessThanField = 15,

        /// <summary>Date major than another date field value</summary>   
        DateMoreThanField = 16,

        /// <summary>Minor than another number field value</summary>   
        NumberMinorThanField = 17,

        /// <summary>Major than another number field value</summary>   
        NumberMajorThanField = 18,

        /// <summary>Minimum of decimal precision</summary>   
        MinLengthDecimal = 19,

        /// <summary>Maximum of decimal precision</summary>   
        MaxLengthDecimal = 20
    }
}