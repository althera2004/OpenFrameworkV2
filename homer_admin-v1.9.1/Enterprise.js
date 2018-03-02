var definition = {
    "ItemName": "Enterprise",
    "MustReload": true,
    "InheritedItems": [
        "Customer",
        "Provider"
    ],
    "Layout": {
        "Icon": "building",
        "Label": "Empresa",
        "LabelPlural": "Empresas",
        "Description": {
            "Pattern": "{0} - {1}",
            "Fields": [{ "Name": "EnterpriseId" }, { "Name": "BusinessName" }]
        }
    },
    "ForeignValues": [
        { "ItemName": "CNAE", "LocalField": "CNAEId", "ForeignField": "Id", "FieldRetrieved": "Description", "ImportReference": "Code" },
        { "ItemName": "Country", "LocalField": "CountryId", "ForeignField": "Id", "FieldRetrieved": "Name" },
        { "ItemName": "Town", "LocalField": "TownId", "ForeignField": "Id", "FieldRetrieved": "Name", "RemoteItem": "Province", "LinkedCombo": "ProvinceId" },
        { "ItemName": "Province", "LocalField": "ProvinceId", "ForeignField": "Id", "FieldRetrieved": "Name", "RemoteItem": "Country", "LinkedCombo": "CountryId" },
        { "ItemName": "ActivitySector", "LocalField": "ActivitySectorId", "ForeignField": "Id", "FieldRetrieved": "Description" }
    ],
    "PrimaryKeys": [
        { "Id": "EnterpriseId", "Values": ["EnterpriseId"] }
    ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "EnterpriseId", "Label": "Id.", "Type": "integer", "Required": true },
        { "Name": "BusinessName", "Label": "Razón social", "Type": "text", "Required": true },
        { "Name": "Tradename", "Label": "Nombre comercial", "Type": "text" },
        { "Name": "ShortName", "Label": "Nombre corto", "Type": "text", "Required": true },
        { "Name": "Adress", "label": "Dirección", "Type": "text" },
        { "Name": "TownId", "Label": "Población", "Type": "long", "ColumnDataType": "Text" },
        { "Name": "PostalCode", "Label": "Código postal", "Type": "text", "ColumnDataType": "Text" },
        { "Name": "ProvinceId", "Label": "Provincia", "Type": "long", "ColumnDataType": "Text" },
        { "Name": "CountryId", "Label": "País", "Type": "long", "ColumnDataType": "Text" },
        { "Name": "Webpage", "Label": "Página web", "Type": "url" },
        { "Name": "Email", "Label": "Correo electrónico", "Type": "email" },
        { "Name": "Phone1", "Label": "Teléfono 1", "Type": "text" },
        { "Name": "Phone2", "Label": "Teléfono 2", "Type": "text" },
        { "Name": "FaxNumber", "Label": "Número fax", "Type": "text" },
        { "Name": "NIF", "Label": "NIF", "Type": "text" },
        { "Name": "FoundationYear", "Label": "Año constitución", "Type": "int" },
        { "Name": "CapitalStock", "Label": "Capital social", "Type": "money", "DataFormat": { "Name": "Money", "Precission": 2 } },
        { "Name": "CompanyGroup", "Label": "Grupo empresarial", "Type": "text" },
        { "Name": "Notes", "Label": "Notas", "Type": "textarea" },
        { "Name": "Agent1Id", "Label": "Agente 1", "Type": "text", "Length": 50 },
        { "Name": "Agent2Id", "Label": "Agente 2", "Type": "text", "Length": 50 },
        { "Name": "ActivitySectorId", "Label": "Id. sector actividad", "Type": "long" },
        { "Name": "Activity", "Label": "Actividad", "Type": "text" },
        { "Name": "CNAEId", "Label": "CNAE", "Type": "long", "ColumnDataType": "Text" },
        { "Name": "EmployeeNumber", "Label": "Nº empleados", "Type": "int" },
        { "Name": "AnualBilling", "Label": "Facturación anual", "Type": "money", "DataFormat": { "Name": "Money", "Precission": 3 } },
        { "Name": "RegularSchedule", "Label": "Horario normal", "Type": "text" },
        { "Name": "SummerSchedule", "Label": "Horario verano", "Type": "text" },
        { "Name": "MarketingSelected", "Label": "Seleccionada márketing", "Type": "boolean", "DataFormat": { "Name": "BooleanCheck" } },
        { "Name": "MarketingSegmentation", "Label": "Segmentación márketing", "Type": "text" },
        { "Name": "IsCustomer", "Label": "Es cliente", "Type": "boolean", "DataFormat": { "Name": "BooleanCheck" } },
        { "Name": "IsProvider", "Label": "Es proveedor", "Type": "boolean", "DataFormat": { "Name": "BooleanCheck" } },
        { "Name": "Blocked", "Label": "Bloqueada", "Type": "boolean", "DataFormat": { "Name": "BooleanCheck" } },
        { "Name": "IsIVACost", "Label": "IVA es gasto", "Type": "boolean", "DataFormat": { "Name": "BooleanCheck" }, "Required": true },
        { "Name": "ProviderApprovalEvaluation", "Label": "Evaluación homologación proveedor", "Type": "text" },
        { "Name": "ProviderApprovalDate", "Label": "Fecha homologación proveedor", "Type": "datetime" },
        { "Name": "ApprovalScore", "Label": "Puntuación homologación", "Type": "decimal", "DataFormat": { "Name": "Decimal", "Precission": 2 } },
        { "Name": "ApprovalScoreUpdateDate", "Label": "Fecha actualización puntuación homologación", "Type": "datetime" },
        { "Name": "EnvironmentalCommitmentDate", "Label": "Fecha compromiso medioambiental", "Type": "datetime" },
        { "Name": "ServerFolder", "Label": "Carpeta servidor", "Type": "text" },
        { "Name": "ClientFolder", "Label": "Carpeta cliente", "Type": "text" },
        { "Name": "AuditorySubfolder", "Label": "Subcarpeta auditoría", "Type": "text" },
        { "Name": "IVACostPercentage", "Label": "Porcentaje gasto IVA", "Type": "decimal", "DataFormat": { "Name": "Decimal", "Precission": 2 }, "Required": true }
    ],
    "SpecialRules": [
        { "FieldNames": ["Email"], "Rule": "Email", "Value": [""] },
        { "FieldNames": ["IVACostPercentage"], "Rule": "RangeValue", "Value": [0, 100] }
    ],
    "Lists": [
        {
            "Id": "Enterprise",
            "Title": "Instancias de O..........F",
            "Layout": 1,
            "EditAction": 3,
            "Duplicate": true,
            "AutoClick": true,
            "Expandible": true,
            "Columns": [
                { "DataProperty": "EnterpriseId" },
                { "DataProperty": "BusinessName", "Label":"Nombre com" },
                { "DataProperty": "Webpage" },
                { "DataProperty": "ProvinceId", "ReplacedBy": "ProvinceDescription" },
                { "DataProperty": "Email" },
                { "DataProperty": "CNAEId", "ToolTip": "Activity" }
            ]
        }
    ],
    "Forms": [
        {
            "Id": "Enterprise",
            "Label": "Empresas",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "General",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "EnterpriseId", "ReadOnly": true },
                                { "Name": "BusinessName", "ReadOnly": true }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Tradename" },
                                { "Name": "ShortName" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Adress" },
                                { "Name": "TownId" },
                                { "Name": "PostalCode" }
                            ]
                        },
                        {
                            "Fields": [

                                { "Name": "ProvinceId" },
                                { "Name": "CountryId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Webpage" },
                                { "Name": "Email" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Phone1" },
                                { "Name": "Phone2" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "FaxNumber" },
                                { "Name": "NIF" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "FoundationYear" },
                                { "Name": "CapitalStock" },
                                { "Name": "CompanyGroup" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Notes", "Rows": 5 }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "IsCustomer" },
                                { "Name": "IsProvider" },
                                { "Name": "Blocked" }

                            ]
                        }
                    ]
                },
                {
                    "Label": "Comercial",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "Agent1Id" },
                                { "Name": "Agent2Id" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "ActivitySectorId" },
                                { "Name": "Activity" },
                                { "Name": "CNAEId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "EmployeeNumber" },
                                { "Name": "AnualBilling" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "RegularSchedule" },
                                { "Name": "SummerSchedule" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "MarketingSelected" },
                                { "Name": "MarketingSegmentation" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Type": 2 },
                                { "Name": "IVACostPercentage" }
                            ]
                        }
                    ]
                },
                {
                    "Label": "Compras",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "ProviderApprovalEvaluation" },
                                { "Name": "ProviderApprovalDate" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "ApprovalScore" },
                                { "Name": "ApprovalScoreUpdateDate" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "EnvironmentalCommitmentDate" }
                            ]
                        }
                    ]
                },
                {
                    "Label": "Varios",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "ServerFolder" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "AuditorySubfolder" }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
};