var items = [
{
    "ItemName": "Actividad",
    "MustReload": true,
  "Layout": {
    "Icon": "list",
    "Label": "Actividad extraescolar",
    "LabelPlural": "Actividades extraescolares",
    "Description": {
      "Pattern": "{0}",
      "Fields": [
        { "Name": "Nombre" }
      ]
    },
    "EditionType": "inline"
  },
  "ForeignValues": [
    { "ItemName": "Proveedor" },
    { "ItemName": "Monitor" },
    { "ItemName": "Lugar" }
  ],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "LugarId", "Label": "Lugar", "Type": "long", "Required": true },
        { "Name": "TipoPago", "Label": "Tipo pago", "Type": "FixedList", "FixedListId": "TipoPagoExtraexcolar", "Required": true, "ColumnDataType": "Text" },
        { "Name": "PrecioUnico", "Label": "Precio único", "Type": "money", "Required": true },
        { "Name": "Precio2", "Label": "Precio 2", "Type": "money" },
        { "Name": "Precio3", "Label": "Precio 3", "Type": "money" },
        { "Name": "ProveedorId", "Label": "Proveedor", "Type": "long", "Required":  true },
        { "Name": "MonitorId", "Label": "Monitor", "Type": "long", "Required": true },
        { "Name": "QuorumMin", "Label": "Mínimo", "Type": "int" },
        { "Name": "QuorumMax", "Label": "Máximo", "Type": "int" }
    ],
    "Lists": [
        {
            "Id": "Custom",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 3,
            "Columns": [
                { "DataProperty": "Nombre" },
                { "DataProperty": "Lugar" }
            ]

        },
        {
            "Id": "ByAlumno",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 3,
            "CustomAjaxSource": "Item_Actividad_GetByAlumno",
            "Columns": [
                { "DataProperty": "Nombre" },
                { "DataProperty": "Lugar" }
            ]

        }
    ],
    "Forms": [
        {
            "Id": "Custom",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "Datos",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "Nombre" },
                                { "Name": "LugarId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "ProveedorId" },
                                { "Name": "MonitorId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "PrecioUnico" },
                                { "Name": "TipoPago" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Precio2" },
                                { "Name": "Precio3" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "QuorumMin" },
                                { "Name": "QuorumMax" }
                            ]
                        }
                    ]
                },
                {
                    "Label": "Inscripciones",
                    "Rows": [
                        {
                            "ItemList": "Inscripcion",
                            "ListId": "Custom",
                            "FilterFields": [ { "Field": "Actividad" } ]
                        }
                    ]
                }
            ]
        }
    ]
},
{
    "ItemName": "Alumno",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Alumno",
        "LabelPlural": "Alumnos",
        "Description": {
            "Pattern": "{0} {1}",
            "Fields": [
                { "Name": "Nombre" },
                { "Name": "Apellidos" }
            ]
        },
        "EditionType": "inline"
    },
  "ForeignValues": [
    { "ItemName": "Curso" },
    { "ItemName": "Tutor" }
  ],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Apellidos", "Label": "Apellidos", "Type": "text", "Length": 100, "Required": true },
        { "Name": "CursoId", "Label": "Curso", "Type": "long", "ColumnDataType": "Text" },
        { "Name": "TutorId", "Label": "Tutor", "Type": "long", "ColumnDataType": "Text", "ColumnDataType": "Text" },
        { "Name": "CuotaAmpa", "Label": "Cuota", "Type": "boolean" , "DataFormat": { "Name": "BooleanCheck" }  }
    ],
    "Lists": [
        {
            "Id": "Custom",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 3,
            "CustomAjaxSource": "ITEM_ALUMNO_GETALL",
            "Columns": [
                { "DataProperty": "Nombre",  "ReplacedBy": "N" },
                { "DataProperty": "CursoId", "ReplacedBy":"C" },
                { "DataProperty": "TutorId", "ReplacedBy":"T" },
                { "DataProperty": "CuotaAmpa", "ReplacedBy": "Cu" }
            ]
        },
        {
            "Id": "ByCurso",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 6,
            "CustomAjaxSource": "ITEM_ALUMNO_GETBYCURSO",
            "Columns": [
                { "DataProperty": "Nombre",  "ReplacedBy": "N" },
                { "DataProperty": "CursoId", "ReplacedBy":"C" },
                { "DataProperty": "TutorId", "ReplacedBy":"T" },
                { "DataProperty": "CuotaAmpa", "ReplacedBy": "Cu" }
            ]
        }
    ],
    "Forms": [
        {
            "Id": "Custom",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "Datos",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "Nombre" },                           
                                { "Name": "Apellidos" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "CursoId" },
                                { "Name": "CuotaAmpa" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "TutorId" }
                            ]
                        }
                    ]
                },
                {
                    "Label": "Inscripciones",
                    "Rows": [
                        {
                            "ItemList": "Inscripcion",
                            "ListId": "ByAlumno",
                            "FilterFields": [ { "Field": "Alumno" } ]
                        }
                    ]
                }
            ]
        }
    ]
},
{
    "ItemName": "Cargo",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Cargo",
        "LabelPlural": "Cargos",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [
		{"ItemName": "Reunion"},
		{"ItemName": "Integrante"}
	],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Fecha", "Label": "Fecha", "Type": "datetime" },
        { "Name": "ReunionId", "Label": "Acta", "Type": "long" },
        { "Name": "IntegranteId", "Label": "Miembro", "Type": "long" ,"Required": true, "ColumnDataType": "Text" }
    ],
    "Lists": [
      {
        "Id": "Custom",
        "FormId": "Custom",
        "Layout": 1,
        "EditAction": 3,
        "CustomAjaxSource": "ITEM_CARGO_GETALL",
        "Columns": [
          {
            "DataProperty": "Nombre",
            "ReplacedBy": "N"
          },
          {
            "DataProperty": "IntegranteId",
            "ReplacedBy": "I"
          }
        ]
      }
    ],
    "Forms": [
        {
            "Id": "Custom",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "Datos",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "Nombre" },                           
                                { "Name": "IntegranteId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "ReunionId" },
                                { "Name": "Fecha" }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
},
{
    "ItemName": "CodigoBanco",
    "MustReload": true,
    "Layout": {
        "Icon": "bank",
        "Label": "Código banco",
        "LabelPlural": "Códigos banco",
        "Description": {
            "Pattern": "{0} - {1}",
            "Fields": [
                { "Name": "Codigo" },
                { "Name": "Banco" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [
		{"ItemName": "Reunion"},
		{"ItemName": "User"}
	],
    "PrimaryKeys": [ { "Id": "BANKCode", "Values": [ "Codigo" ] } ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Codigo", "Label": "Código", "Type": "text", "Length": 4, "Required": true },
        { "Name": "Banco", "Label": "Banco", "Type": "text", "length": 100, "Required": true }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
    "ItemName": "Curso",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Curso",
        "LabelPlural": "Cursos",
        "Description": {
            "Pattern": "{0} {1}",
            "Fields": [
                { "Name": "Nombre" },
                { "Name": "Apellidos" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Curso", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Externo", "Label": "Externo", "Type": "boolean", "DataFormat": { "Name": "BooleanCheck" } }
    ],
    "Lists": [
        {
            "Id": "Custom",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 3,
            "CustomAjaxSource": "ITEM_CURSO_GETALL",
            "Columns": [
                { "DataProperty": "Nombre",  "ReplacedBy": "N" },
                { "DataProperty": "Externo", "ReplacedBy": "E" }
            ]
        }
    ],
    "Forms": [
        {
            "Id": "Custom",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "Datos",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "Nombre" },                           
                                { "Name": "Externo" }
                            ]
                        },
                        {
                            "ItemList": "Alumno",
                            "ListId": "ByCurso",
                            "FilterFields": [ { "Field": "Curso" } ]
                        }
                    ]
                }
            ]
        }
    ]
},
{
    "ItemName": "HistoricoCargo",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Histórico cargo",
        "LabelPlural": "Histórico Cargos",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [
		{"ItemName": "Cargo"}
	],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "CargoId", "Label": "Cargo", "Type": "long", "Required": true },
        { "Name": "FechaIni", "Label": "Inicio", "Type": "datetime", "Required": true },
        { "Name": "FechaFin", "Label": "Final", "Type": "datetime", "Required": true },
        { "Name": "Persona", "Label": "Persona", "Type": "text" ,"Required": true, "Length": 100 }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
    "ItemName": "Inscripcion",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Inscripción",
        "LabelPlural": "Inscripciones",
        "Description": {
            "Pattern": "{0} {1}",
            "Fields": [
                { "Name": "Nombre" },
                { "Name": "Apellidos" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [
        { "ItemName": "Actividad" },
        { "ItemName": "Alumno" }
    ],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "AlumnoId", "Label": "Alumno", "Type": "long", "Required": true, "ColumnDataType": "text" },
        { "Name": "cc", "Label": "C.C.", "Type": "text", "Length": 30 },
        { "Name": "ActividadId", "Label": "Actividad", "Type": "long", "Required": true, "ColumnDataType": "text" },
        { "Name": "FechaInscripcion", "Label": "Inscripción", "Type": "datetime", "Required": true },
        { "Name": "FechaConfirmacion", "Label": "Confirmación", "Type": "datetime" },
        { "Name": "FechaBaja", "Label": "Baja", "Type": "datetime" },
        { "Name": "MotivoBaja", "Label": "Motivo", "Type": "textarea", "Length": 500 }
    ],
    "Lists": [
        {
            "Id": "Custom",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 3,
            "Columns": [
                { "DataProperty": "AlumnoId" },
                { "DataProperty": "FechaInscripcion" },
                { "DataProperty": "FechaConfirmacion" }
            ]

        }
    ],
    "Forms": [
        {
            "Id": "Custom",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "Datos",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "ActividadId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "AlumnoId" },
                                { "Name": "FechaInscripcion" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "FechaConfirmacion" },
                                { "Name": "cc" }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
},
{
    "ItemName": "Integrante",
    "MustReload": true,
    "Layout": {
        "Icon": "user",
        "Label": "Integrante",
        "LabelPlural": "Integrantes",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [ ],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Email", "Label": "Email", "Type": "email", "Length": 100, "Required": true },
        { "Name": "Fecha", "Label": "Fecha", "Type": "datetime" },
        { "Name": "Externo", "Label": "Externo", "Type": "bool", "DataFormat": { "Name": "BooleanCheck" } }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
  "ItemName": "Lugar",
  "MustReload": true,
  "Layout": {
    "Icon": "list",
    "Label": "Lugar",
    "LabelPlural": "Lugares",
    "Description": {
      "Pattern": "{0}",
      "Fields": [
        { "Name": "Nombre" }
      ]
    },
    "EditionType": "inline"
  },
  "ForeignValues": [
    { "ItemName": "Ubicacion" }
  ],
  "PrimaryKeys": [],
  "Fields": [
    {
      "Name": "Id",
      "Type": "long",
      "Required": true
    },
    {
      "Name": "Nombre",
      "Label": "Nombre",
      "Type": "text",
      "Length": 100,
      "Required": true
    },
    {
      "Name": "UbicacionId",
      "Label": "Ubicacion",
      "Type": "long",
      "Required": true
    }
  ],
  "Lists": [
    {
      "Id": "Custom",
      "FormId": "Custom",
      "Layout": 1,
      "EditAction": 3,
      "CustomAjaxSource": "ITEM_LUGAR_GETALL",
      "Columns": [
        {
          "DataProperty": "Nombre",
          "ReplacedBy": "N"
        },
        {
          "DataProperty": "UbicacionId",
          "ReplacedBy": "U"
        }
      ]
    }
  ],
  "Forms": [
    {
      "Id": "Custom",
      "FormType": "Custom",
      "Tabs": [
        {
          "Label": "Datos",
          "Rows": [
            {
              "Fields": [
                { "Name": "Nombre" },
                { "Name": "UbicacionId" }
              ]
            }
          ]
        }
      ]
    }
  ]
},
{
    "ItemName": "Monitor",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Monitor",
        "LabelPlural": "Monitores",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [
		{"ItemName": "Proveedor"}
	],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Telefono", "Label": "Telèfon", "Type": "text", "Length": 15, "Required": true },
        { "Name": "Email", "Label": "Email", "Type": "text", "Length": 100, "Required": true },
        { "Name": "ProveedorId", "Label": "Proveedor", "Type": "long" }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
    "ItemName": "Noticia",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Noticia",
        "LabelPlural": "Noticias",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Titulo" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Titulo", "Label": "Titulo", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Fecha", "Label": "Fecha", "Type": "date", "Required": true }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
    "ItemName": "Proveedor",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Proveedor",
        "LabelPlural": "Proveedores",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [	],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Direccion", "Label": "Dirección", "Type": "text", "Required": true },
        { "Name": "Telefono", "Label": "Teléfono", "Type": "text" ,"Required": true },
        { "Name": "Email", "Label": "Reunion", "Type": "email" },
        { "Name": "Web", "Label": "Web", "Type": "url" },
        { "Name": "CC", "Label": "CC", "Type": "text", "Length": 50 }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
    "ItemName": "ReunionPunto",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Punto reunión",
        "LabelPlural": "Puntos reunión",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Titulo" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [ {"ItemName": "Reunion"} ],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Titulo", "Label": "Titulo", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Descripcion", "Label": "Descripcion", "Type": "text", "Length": 1000 },
        { "Name": "Orden", "Label": "Orden", "Type": "integer", "Required": true },
        { "Name": "ReunionId", "Label": "Reunión", "Type": "long", "Required": true }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
    "ItemName": "Reunion",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Reunión",
        "LabelPlural": "Reuniones",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [ {"ItemName": "Integrante"}, { "ItemName": "ReunionTipo"} ],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "FechaConvocacion", "Label": "Convocación", "Type": "datetime" },
        { "Name": "FechaConvocatoria", "Label": "Convocatoria", "Type": "datetime" },
        { "Name": "IntegranteId", "Label": "Convocante", "Type": "long", "Required": true, "ColumnDataType": "Text" },
        { "Name": "ReunionTipoId", "Label": "Tipo", "Type": "long", "Required": true, "ColumnDataType": "Text" },
        { "Name": "Abierta", "Label": "Abierta", "Type": "bool" }
    ],
  "Lists": [
    {
      "Id": "Custom",
      "FormId": "Custom",
      "Layout": 1,
      "EditAction": 3,
      "CustomAjaxSource": "ITEM_REUNION_GETALL",
      "Columns": [
        {
          "DataProperty": "Nombre",
          "ReplacedBy": "N"
        },
        {
          "DataProperty": "FechaConvocatoria",
          "ReplacedBy": "F"
        },
        {
          "DataProperty": "ReunionTipoId",
          "ReplacedBy": "T"
        },
        {
          "DataProperty": "IntegranteId",
          "ReplacedBy": "I"
        }
      ]
    }
  ],
  "Forms": [
    {
      "Id": "Custom",
      "FormType": "Custom",
      "Tabs": [
        {
          "Label": "Datos",
          "Rows": [
            {
              "Fields": [
                { "Name": "Nombre" },
                { "Name": "IntegranteId" }
              ]
            },
            {
              "Fields": [
                { "Name": "FechaConvocacion" },
                { "Name": "FechaConvocatoria" }
              ]
            },
            {
              "Fields": [
                { "Name": "ReunionTipoId" },
                { "Name": "Abierta" }
              ]
            }
          ]
        }
      ]
    }
  ]
},
{
    "ItemName": "ReunionPeriodicidad",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Preiodicidad reunión",
        "LabelPlural": "Periodididad reuniones",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Tipo", "Type": "text", "Length": 100, "Required": true }
    ],
    "Lists": [ ],
    "Forms": [ ]
},
{
    "ItemName": "ReunionTipo",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Tipo reunión",
        "LabelPlural": "Tipos reunión",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [{"ItemName": "ReunionPeriodicidad"}],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Tipo", "Type": "text", "Length": 100, "Required": true },
        { "Name": "ReunionPeriodicidadId", "Label": "Periodicidad", "Type": "long", "ColumnDataType": "Text" }
    ],
    "Lists": [
        {
            "Id": "Custom",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 3,
            "CustomAjaxSource": "Item_ReunionTipo_GetAll",
            "Columns": [
                { "DataProperty": "Nombre", "ReplacedBy":"N" },
                { "DataProperty": "ReunionPeriodicidadId", "ReplacedBy":"P" }
            ]

        } ],
    "Forms": [
      {
        "Id": "Custom",
        "FormType": "Custom",
        "Tabs": [
          {
            "Label": "Datos",
            "Rows": [
              {
                "Fields": [
                  { "Name": "Nombre" },
                  { "Name": "ReunionPeriodicidadId" }
                ]
              }
            ]
          }
        ]
      }
    ]
},
{
    "ItemName": "Tutor",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Tutor",
        "LabelPlural": "Tutores de alumnos",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre1" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [],
    "PrimaryKeys": [],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Email", "Label": "Email", "Type": "email", "Required": true, "Length": 50 },
        { "Name": "Email2", "Label": "Email2", "Type": "email", "Length": 50 },
        { "Name": "Nombre1", "Label": "Nombre 1", "Type": "text", "Required": true },
        { "Name": "DNI1", "Label": "DNI 1", "Type": "text", "Required": true, "Length": 12  },
        { "Name": "Telefono1", "Label":  "Teléfono 1", "Type": "text", "Length": 15 },
        { "Name": "Nombre2", "Label": "Nombre2", "Type": "text", "Length": 100 },
        { "Name": "DNI2", "Label": "DNI 2", "Type": "text" , "Length": 12},
        { "Name": "Telefono2", "Label":  "Teléfono 2", "Type": "text", "Length": 15 },
        { "Name": "Nombre3", "Label": "Nombre3", "Type": "text", "Length": 100 },
        { "Name": "DNI3", "Label": "DNI 3", "Type": "text", "Length": 12 },
        { "Name": "Telefono3", "Label":  "Teléfono 3", "Type": "text", "Length": 15 },
        { "Name": "CCExtraescolar", "Label":  "C.C.", "Type": "text", "Length": 50 }
    ],
    "Lists": [
        {
            "Id": "Custom",
            "FormId": "Custom",
            "Layout": 1,
            "EditAction": 3,
            "Columns": [
                { "DataProperty": "Email" },
                { "DataProperty": "Nombre1" },
                { "DataProperty": "Telefono1" },
                { "DataProperty": "Nombre2" },
                { "DataProperty": "Telefono2" }
            ]

        }
    ],
    "Forms": [
        {
            "Id": "Custom",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "Datos",
                    "Rows": [
                        {
                            "Label": "Primer contacto (obligatorio)",
                            "Fields": [
                                { "Name": "Nombre1" },
                                { "Name": "DNI1" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Telefono1" },
                                { "Name": "Email" }
                            ]
                        },
                        {
                            "Label": "Segundo contacto <i>(opcional)</i>",
                            "Fields": [
                                { "Name": "Nombre2" },
                                { "Name": "DNI2" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Telefono2" },
                                { "Name": "Email2" }
                            ]
                        },
                        {
                            "Label": "Tercer contacto <i>(opcional)</i>",
                            "Fields": [
                                { "Name": "Nombre3" },
                                { "Name": "DNI3" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Telefono3" },
                                { "Type": 2 }
                            ]
                        },
                        {
                            "Label": "C.C. para domiciliar cuota AMPA y actividades extraescolares",
                            "Fields": [
                                { "Name": "CCExtraescolar" }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
},
{
    "ItemName": "Ubicacion",
    "MustReload": true,
    "Layout": {
        "Icon": "list",
        "Label": "Ubicación",
        "LabelPlural": "Ubicaciones",
        "Description": {
            "Pattern": "{0}",
            "Fields": [
                { "Name": "Nombre" }
            ]
        },
        "EditionType": "inline"
    },
    "ForeignValues": [
		{"ItemName": "Reunion"},
		{"ItemName": "User"}
	],
    "PrimaryKeys": [ ],
    "Fields": [
        { "Name": "Id", "Type": "long", "Required": true },
        { "Name": "Nombre", "Label": "Nombre", "Type": "text", "Length": 100, "Required": true },
        { "Name": "Propio", "Label": "Propio", "Type": "boolean", "DataFormat": { "Name": "BooleanCheck" } }
    ],
    "Lists": [ ],
    "Forms": [ ]
}];
