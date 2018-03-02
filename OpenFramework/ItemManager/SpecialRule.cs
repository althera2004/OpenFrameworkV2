// --------------------------------
// <copyright file="SpecialRule.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using OpenFramework.Core;

    public sealed class SpecialRule
    {
        private ItemBuilder item;
        private ItemFieldRules rule;

        public SpecialRule(ItemBuilder item, ItemFieldRules rule)
        {
            this.item = item;
            this.rule = rule;
        }

        public ActionResult Complains
        {
            get
            {
                var res = ActionResult.NoAction;
                bool ok = true;
                StringBuilder message = new StringBuilder();
                try
                {
                    List<ItemField> fields = this.item.Definition.Fields.Where(f => this.rule.FieldNames.Contains(f.Name)).ToList();

                    if (fields == null)
                    {
                        res.SetSuccess();
                        return res;
                    }

                    if (fields.Count == 0)
                    {
                        res.SetSuccess();
                        return res;
                    }

                    switch (this.rule.Rule)
                    {
                        case ValidatorRule.RangeValue:
                            ActionResult resRange = this.ComplainsRange(fields[0]);
                            if (!resRange.Success)
                            {
                                ok = false;
                                message.Append(resRange.MessageError);
                            }

                            break;
                        case ValidatorRule.MaxValue:
                            ActionResult resMaxValue = this.ComplainsMaximumValue(fields[0]);
                            if (!resMaxValue.Success)
                            {
                                ok = false;
                                message.Append(resMaxValue.MessageError);
                            }

                            break;
                        case ValidatorRule.MinValue:
                            ActionResult resMinValue = this.ComplainsMinimumValue(fields[0]);
                            if (!resMinValue.Success)
                            {
                                ok = false;
                                message.Append(resMinValue.MessageError);
                            }

                            break;
                        case ValidatorRule.OnlyNumbers:
                            ActionResult resOnlyNumbers = this.ComplainsOnlyNumbers(fields[0]);
                            if (!resOnlyNumbers.Success)
                            {
                                ok = false;
                                message.Append(resOnlyNumbers.MessageError);
                            }

                            break;
                        case ValidatorRule.MaxLength:
                            ActionResult resMaxLength = this.MaxLength(fields[0]);
                            if (!resMaxLength.Success)
                            {
                                ok = false;
                                message.Append(resMaxLength.MessageError);
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {
                    res.SetFail(ex);
                    return res;
                }

                if (ok)
                {
                    res.SetSuccess();
                }
                else
                {
                    res.SetFail(message.ToString());
                }

                return res;
            }
        }

        private ActionResult ComplainsRange(ItemField field)
        {
            ActionResult res = ActionResult.SuccessNoAction;
            if (this.item.ContainsKey(field.Name))
            {
                switch (field.DataType)
                {
                    case FieldDataType.NullableInteger:
                        int valueInteger = Convert.ToInt32(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = !(valueInteger < Convert.ToInt32(this.rule.Value[0], CultureInfo.InvariantCulture) || valueInteger > Convert.ToInt32(this.rule.Value[1], CultureInfo.InvariantCulture));
                        break;
                    case FieldDataType.Integer:
                        res.Success = !(Convert.ToInt32(this.item[field.Name], CultureInfo.InvariantCulture) < Convert.ToInt32(this.rule.Value[0], CultureInfo.InvariantCulture) || Convert.ToInt32(this.item[field.Name], CultureInfo.InvariantCulture) > Convert.ToInt32(this.rule.Value[1], CultureInfo.InvariantCulture));
                        break;
                    case FieldDataType.NullableDecimal:
                        decimal valueDecimal = Convert.ToDecimal(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = !(valueDecimal < Convert.ToDecimal(this.rule.Value[0], CultureInfo.InvariantCulture) || valueDecimal > Convert.ToDecimal(this.rule.Value[1], CultureInfo.InvariantCulture));
                        break;
                    case FieldDataType.Decimal:
                        if (field.Required && string.IsNullOrEmpty(this.item[field.Name].ToString()))
                        {
                            res.Success = false;
                        }
                        else
                        {
                            if (!field.Required && string.IsNullOrEmpty(this.item[field.Name].ToString()))
                            {
                                res.Success = true;
                            }
                            else
                            {
                                res.Success = !(Convert.ToDecimal(this.item[field.Name], CultureInfo.InvariantCulture) < Convert.ToDecimal(this.rule.Value[0], CultureInfo.InvariantCulture) || Convert.ToDecimal(this.item[field.Name], CultureInfo.InvariantCulture) > Convert.ToDecimal(this.rule.Value[1], CultureInfo.InvariantCulture));
                            }
                        }

                        break;
                    case FieldDataType.NullableLong:
                        if (this.item[field.Name].GetType().Name.ToUpperInvariant() == "STRING")
                        {
                            if (string.IsNullOrEmpty(this.item[field.Name] as string))
                            {
                                res.Success = true;
                            }
                            else
                            {
                                long valueLong = Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture);
                                res.Success = !(valueLong < Convert.ToInt64(this.rule.Value[0], CultureInfo.InvariantCulture) || valueLong > Convert.ToInt64(this.rule.Value[1], CultureInfo.InvariantCulture));
                            }
                        }
                        else
                        {
                            long valueLong = Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture);
                            res.Success = !(valueLong < Convert.ToInt64(this.rule.Value[0], CultureInfo.InvariantCulture) || valueLong > Convert.ToInt64(this.rule.Value[1], CultureInfo.InvariantCulture));
                        }
                        
                        break;
                    case FieldDataType.Long:
                        res.Success = !(Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture) < Convert.ToInt64(this.rule.Value[0], CultureInfo.InvariantCulture) || Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture) > Convert.ToInt64(this.rule.Value[1], CultureInfo.InvariantCulture));
                        break;
                    case FieldDataType.NullableFloat:
                        float valueFloat = Convert.ToSingle(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = !(valueFloat < Convert.ToSingle(this.rule.Value[0], CultureInfo.InvariantCulture) || valueFloat > Convert.ToSingle(this.rule.Value[1], CultureInfo.InvariantCulture));
                        break;
                    case FieldDataType.Float:
                        res.Success = !(Convert.ToSingle(this.item[field.Name], CultureInfo.InvariantCulture) < Convert.ToSingle(this.rule.Value[0], CultureInfo.InvariantCulture) || Convert.ToSingle(this.item[field.Name], CultureInfo.InvariantCulture) > Convert.ToSingle(this.rule.Value[1], CultureInfo.InvariantCulture));
                        break;
                    default:
                        res.Success = false;
                        res.MessageError = string.Format(CultureInfo.InvariantCulture, "El tipo de dato de {0} no es correcto", field.Label);
                        break;
                }

                if (!res.Success && string.IsNullOrEmpty(res.MessageError))
                {
                    res.SetFail(string.Format(
                        CultureInfo.InvariantCulture,
                        @"El valor de {0} debe estar entre {1} y {2}.",
                        field.Label,
                        this.rule.Value[0],
                        this.rule.Value[1]));
                }
            }

            return res;
        }

        private ActionResult ComplainsMinimumValue(ItemField field)
        {
            ActionResult res = ActionResult.SuccessNoAction;
            if (this.item.ContainsKey(field.Name))
            {
                switch (field.DataType)
                {
                    case FieldDataType.NullableInteger:
                        int valueInteger = Convert.ToInt32(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueInteger >= Convert.ToInt32(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Integer:
                        res.Success = Convert.ToInt32(this.item[field.Name], CultureInfo.InvariantCulture) >= Convert.ToInt32(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.NullableDecimal:
                        decimal valueDecimal = Convert.ToDecimal(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueDecimal >= Convert.ToDecimal(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Decimal:
                        res.Success = Convert.ToDecimal(this.item[field.Name], CultureInfo.InvariantCulture) >= Convert.ToDecimal(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.NullableLong:
                        long valueLong = Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueLong >= Convert.ToInt64(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Long:
                        res.Success = Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture) >= Convert.ToInt64(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.NullableFloat:
                        float valueFloat = Convert.ToSingle(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueFloat >= Convert.ToSingle(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Float:
                        res.Success = Convert.ToSingle(this.item[field.Name], CultureInfo.InvariantCulture) >= Convert.ToSingle(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    default:
                        res.Success = false;
                        res.MessageError = string.Format(CultureInfo.InvariantCulture, "El tipo de dato de {0} no es correcto", field.Label);
                        break;
                }

                if (!res.Success && string.IsNullOrEmpty(res.MessageError))
                {
                    res.SetFail(string.Format(CultureInfo.InvariantCulture, @"El valor de {0} es menor que {1}.", field.Label, this.rule.Value[0]));
                }
            }

            return res;
        }

        private ActionResult ComplainsMaximumValue(ItemField field)
        {
            ActionResult res = ActionResult.SuccessNoAction;
            if (this.item.ContainsKey(field.Name))
            {
                switch (field.DataType)
                {
                    case FieldDataType.NullableInteger:
                        int valueInteger = Convert.ToInt32(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueInteger <= Convert.ToInt32(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Integer:
                        res.Success = Convert.ToInt32(this.item[field.Name], CultureInfo.InvariantCulture) <= Convert.ToInt32(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.NullableDecimal:
                        decimal valueDecimal = Convert.ToDecimal(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueDecimal <= Convert.ToDecimal(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Decimal:
                        res.Success = Convert.ToDecimal(this.item[field.Name], CultureInfo.InvariantCulture) <= Convert.ToDecimal(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.NullableLong:
                        long valueLong = Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueLong <= Convert.ToInt64(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Long:
                        res.Success = Convert.ToInt64(this.item[field.Name], CultureInfo.InvariantCulture) <= Convert.ToInt64(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.NullableFloat:
                        float valueFloat = Convert.ToSingle(this.item[field.Name], CultureInfo.InvariantCulture);
                        res.Success = valueFloat <= Convert.ToSingle(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    case FieldDataType.Float:
                        res.Success = Convert.ToSingle(this.item[field.Name], CultureInfo.InvariantCulture) <= Convert.ToSingle(this.rule.Value[0], CultureInfo.InvariantCulture);
                        break;
                    default:
                        res.Success = false;
                        res.MessageError = string.Format(CultureInfo.InvariantCulture, "El tipo de dato de {0} no es correcto", field.Label);
                        break;
                }

                if (!res.Success && string.IsNullOrEmpty(res.MessageError))
                {
                    res.SetFail(string.Format(CultureInfo.InvariantCulture, @"El valor de {0} es mayor que {1}.", field.Label, this.rule.Value[0]));
                }
            }

            return res;
        }

        private ActionResult MaxLength(ItemField field)
        {
            ActionResult res = ActionResult.SuccessNoAction;
            if (field == null)
            {
                res.SetSuccess();
                return res;
            }

            if (this.item[field.Name] == null)
            {
                res.SetSuccess();
                return res;
            }

            string value = this.item[field.Name] as string;
            value = value.Trim();

            if (string.IsNullOrEmpty(value))
            {
                res.SetSuccess();
            }
            else
            {
                if (value.Length > (long)this.rule.Value[0])
                {
                    res.SetFail(string.Format(
                    CultureInfo.InvariantCulture,
                    @"El valor de {0} debe ser como máximo de {1} caracter{2}.",
                    field.Label,
                    this.rule.Value[0],
                    Convert.ToInt64(this.rule.Value[0]) == 1 ? string.Empty : "es"));
                }
                else
                {
                    res.SetSuccess();
                }
            }

            return res;
        }

        private ActionResult ComplainsOnlyNumbers(ItemField field)
        {
            ActionResult res = ActionResult.SuccessNoAction;

            if (field == null)
            {
                return res;
            }

            string value = this.item[field.Name] as string;
            if (!string.IsNullOrEmpty(value))
            {
                Regex regex = new Regex(@"^[0-9]+$");
                if (regex.IsMatch(value))
                {
                    res.SetSuccess();
                }
                else
                {
                    res.SetFail(string.Format(
                    CultureInfo.InvariantCulture,
                    @"El valor de {0} debe contener sólo números.",
                    field.Label));
                }
            }

            return res;
        }
    }
}