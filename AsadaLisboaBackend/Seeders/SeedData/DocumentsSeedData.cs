using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.Seeders.SeedData;

/// <summary>
/// Provides seed data for documents.
/// </summary>
public static class DocumentsSeedData
{
    /// <summary>
    /// Gets the list of seed documents.
    /// </summary>
    /// <returns>A list of seed documents.</returns>
    public static List<Document> Get()
    {
        return new()
        {
            new ()
            {
                Id = Guid.Parse("017b65c0-6a58-4e56-919f-6d2de0782738"),
                Slug = "balance-general-2022-017b65",
                Title = "Balance General 2022",
                Description = "Balance general financiero correspondiente al período 2022.",
                PublicationDate = DateTime.Parse("2022-12-01T00:00:00Z").ToUniversalTime(),
                FileSize = 8236648,
                Url = "/documentos/balance-general-2022-017b65.pdf",
                FileName = "balance-general-2022-017b65.pdf",
                FilePath = "documentos/balance-general-2022-017b65.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("0f911392-11a1-4985-8a7e-db4fe19ad029"),
                Slug = "balance-general-2023-0f9113",
                Title = "Balance General 2023",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Balance general financiero correspondiente al período 2023.",
                FileSize = 8238773,
                Url = "/documentos/balance-general-2023-0f9113.pdf",
                FileName = "balance-general-2023-0f9113.pdf",
                FilePath = "documentos/balance-general-2023-0f9113.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("61d28a90-16fa-46cb-bea4-afa6becd20f7"),
                Slug = "estudio-de-demanda-del-pozo-61d28a",
                Title = "Estudio de Demanda del Pozo",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Estudio técnico sobre la demanda y capacidad de abastecimiento del pozo principal.",
                FileSize = 159287,
                Url = "/documentos/estudio-de-demanda-del-pozo-61d28a.docx",
                FileName = "estudio-de-demanda-del-pozo-61d28a.docx",
                FilePath = "documentos/estudio-de-demanda-del-pozo-61d28a.docx",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("9029EB69-0E77-4CF6-8622-C7902D565745")
            },
            new ()
            {
                Id = Guid.Parse("4d0d90f5-45d1-4e7b-83b6-0f1b5f3ed934"),
                Slug = "informe-ejecutivo-anual-contable-2022-4d0d90",
                Title = "Informe Ejecutivo Anual Contable 2022",
                PublicationDate = DateTime.Parse("2022-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe ejecutivo anual con resultados y gestión contable del período 2022.",
                FileSize = 155299,
                Url = "/documentos/informe-ejecutivo-anual-contable-2022-4d0d90.pdf",
                FileName = "informe-ejecutivo-anual-contable-2022-4d0d90.pdf",
                FilePath = "documentos/informe-ejecutivo-anual-contable-2022-4d0d90.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("86300c54-6634-49b8-98fa-5c4522b1c1a8"),
                Slug = "informe-ejecutivo-anual-contable-2023-86300c",
                Title = "Informe Ejecutivo Anual Contable 2023",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe ejecutivo anual con resultados y gestión contable del período 2023.",
                FileSize = 80533,
                Url = "/documentos/informe-ejecutivo-anual-contable-2023-86300c.pdf",
                FileName = "informe-ejecutivo-anual-contable-2023-86300c.pdf",
                FilePath = "documentos/informe-ejecutivo-anual-contable-2023-86300c.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("ad0bc67d-0b44-4f75-bb44-b6734269e9c7"),
                Slug = "informe-fiscal-2022-ad0bc6",
                Title = "Informe Fiscal 2022",
                PublicationDate = DateTime.Parse("2022-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe fiscal correspondiente a la gestión administrativa y financiera del año 2022.",
                FileSize = 144860,
                Url = "/documentos/informe-fiscal-2022-ad0bc6.pdf",
                FileName = "informe-fiscal-2022-ad0bc6.pdf",
                FilePath = "documentos/informe-fiscal-2022-ad0bc6.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("bc52f228-10cf-4fff-9db7-497e02e716b2"),
                Slug = "informe-fiscal-2023-asamblea-general-ordinaria-y-extraordinaria-bc52f2",
                Title = "Informe Fiscal 2023 Asamblea General Ordinaria y Extraordinaria",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe fiscal presentado en la asamblea general ordinaria y extraordinaria del año 2023.",
                FileSize = 145649,
                Url = "/documentos/informe-fiscal-2023-asamblea-general-ordinaria-y-extraordinaria-bc52f2.pdf",
                FileName = "informe-fiscal-2023-asamblea-general-ordinaria-y-extraordinaria-bc52f2.pdf",
                FilePath = "documentos/informe-fiscal-2023-asamblea-general-ordinaria-y-extraordinaria-bc52f2.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("7893c468-2309-4433-b9d7-87fa7ae84da6"),
                Slug = "informe-plan-de-trabajo-e-inversin-2023-7893c4",
                Title = "Informe Plan de Trabajo  e Inversión 2023",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Plan de trabajo e inversión con objetivos y proyectos correspondientes al período 2023.",
                FileSize = 129260,
                Url = "/documentos/informe-plan-de-trabajo-e-inversin-2023-7893c4.pdf",
                FileName = "informe-plan-de-trabajo-e-inversin-2023-7893c4.pdf",
                FilePath = "documentos/informe-plan-de-trabajo-e-inversin-2023-7893c4.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("64cfb69b-b06e-4d30-b4b3-0190a66a1037"),
                Slug = "informe-presidencial-2022-64cfb6",
                Title = "Informe Presidencial 2022",
                PublicationDate = DateTime.Parse("2022-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe presidencial sobre la gestión institucional realizada durante el año 2022.",
                FileSize = 79144,
                Url = "/documentos/informe-presidencial-2022-64cfb6.pdf",
                FileName = "informe-presidencial-2022-64cfb6.pdf",
                FilePath = "documentos/informe-presidencial-2022-64cfb6.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("4655903f-2404-4fa3-8e21-cbe909d123fe"),
                Slug = "informe-presidencial-2023-465590",
                Title = "Informe Presidencial 2023",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe presidencial sobre la gestión institucional realizada durante el año 2023.",
                FileSize = 75535,
                Url = "/documentos/informe-presidencial-2023-465590.pdf",
                FileName = "informe-presidencial-2023-465590.pdf",
                FilePath = "documentos/informe-presidencial-2023-465590.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("52c96608-af95-4917-bcdb-ec4e34ef077c"),
                Slug = "informe-tesorera-2022-52c966",
                Title = "Informe Tesorería 2022",
                PublicationDate = DateTime.Parse("2022-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe de tesorería con detalles financieros y administrativos correspondientes al año 2022.",
                FileSize = 108359,
                Url = "/documentos/informe-tesorera-2022-52c966.pdf",
                FileName = "informe-tesorera-2022-52c966.pdf",
                FilePath = "documentos/informe-tesorera-2022-52c966.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("99058f1a-b892-4ae5-a15f-530fd247c57c"),
                Slug = "lineamiento-para-consumo-estimado-99058f",
                Title = "Lineamiento para Consumo Estimado",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Lineamientos técnicos y administrativos para la estimación del consumo de agua.",
                FileSize = 164184,
                Url = "/documentos/lineamiento-para-consumo-estimado-99058f.pdf",
                FileName = "lineamiento-para-consumo-estimado-99058f.pdf",
                FilePath = "documentos/lineamiento-para-consumo-estimado-99058f.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("ad5f4c60-a71f-4f9b-bfb1-645fd54a9d3f"),
                Slug = "perfil-del-pozo-principal-ad5f4c",
                Title = "Perfil del Pozo Principal",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Documento técnico con información general y características del pozo principal.",
                FileSize = 81995,
                Url = "/documentos/perfil-del-pozo-principal-ad5f4c.docx",
                FileName = "perfil-del-pozo-principal-ad5f4c.docx",
                FilePath = "documentos/perfil-del-pozo-principal-ad5f4c.docx",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("9029EB69-0E77-4CF6-8622-C7902D565745")
            },
            new ()
            {
                Id = Guid.Parse("c4cafcf9-9d17-4170-b52e-55232d7a7b4d"),
                Slug = "pozo-principal-exmenes-bacteriolgicos-julio-2023-c4cafc",
                Title = "Pozo Principal - Exámenes Bacteriológicos Julio 2023",
                PublicationDate = DateTime.Parse("2023-07-01T00:00:00Z").ToUniversalTime(),
                Description = "Resultados de análisis bacteriológicos realizados al pozo principal en julio de 2023.",
                FileSize = 3159633,
                Url = "/documentos/pozo-principal-exmenes-bacteriolgicos-julio-2023-c4cafc.pdf",
                FileName = "pozo-principal-exmenes-bacteriolgicos-julio-2023-c4cafc.pdf",
                FilePath = "documentos/pozo-principal-exmenes-bacteriolgicos-julio-2023-c4cafc.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("0b9ad537-f55c-484a-ae73-5c2adaac6ce3"),
                Slug = "pozo-principal-exmenes-fsico-qumicos-julio-2023-0b9ad5",
                Title = "Pozo Principal - Exámenes Físico-Químicos Julio 2023",
                PublicationDate = DateTime.Parse("2023-07-01T00:00:00Z").ToUniversalTime(),
                Description = "Resultados de análisis físico-químicos realizados al pozo principal en julio de 2023.",
                FileSize = 2210333,
                Url = "/documentos/pozo-principal-exmenes-fsico-qumicos-julio-2023-0b9ad5.pdf",
                FileName = "pozo-principal-exmenes-fsico-qumicos-julio-2023-0b9ad5.pdf",
                FilePath = "documentos/pozo-principal-exmenes-fsico-qumicos-julio-2023-0b9ad5.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("829dc314-5c88-4ed6-b83c-838ac005276a"),
                Slug = "pozo-principal-informe-abril-2020-829dc3",
                Title = "Pozo Principal - Informe Abril 2020",
                PublicationDate = DateTime.Parse("2020-04-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a abril de 2020.",
                FileSize = 1081856,
                Url = "/documentos/pozo-principal-informe-abril-2020-829dc3.doc",
                FileName = "pozo-principal-informe-abril-2020-829dc3.doc",
                FilePath = "documentos/pozo-principal-informe-abril-2020-829dc3.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("1f1ef617-1a46-4825-8ad5-6d144a69835d"),
                Slug = "pozo-principal-informe-abril-2021-1f1ef6",
                Title = "Pozo Principal - Informe Abril 2021",
                PublicationDate = DateTime.Parse("2021-04-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a abril de 2021.",
                FileSize = 1569200,
                Url = "/documentos/pozo-principal-informe-abril-2021-1f1ef6.pdf",
                FileName = "pozo-principal-informe-abril-2021-1f1ef6.pdf",
                FilePath = "documentos/pozo-principal-informe-abril-2021-1f1ef6.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("27ea0641-00e8-4a45-9651-346b153cf672"),
                Slug = "pozo-principal-informe-agosto-2015-27ea06",
                Title = "Pozo Principal - Informe Agosto 2015",
                PublicationDate = DateTime.Parse("2015-08-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a agosto de 2015.",
                FileSize = 1082880,
                Url = "/documentos/pozo-principal-informe-agosto-2015-27ea06.doc",
                FileName = "pozo-principal-informe-agosto-2015-27ea06.doc",
                FilePath = "documentos/pozo-principal-informe-agosto-2015-27ea06.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("96ecb1ce-fc8c-4d76-8799-92ca90b14603"),
                Slug = "pozo-principal-informe-diciembre-2019-96ecb1",
                Title = "Pozo Principal - Informe Diciembre 2019",
                PublicationDate = DateTime.Parse("2019-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a diciembre de 2019.",
                FileSize = 1081344,
                Url = "/documentos/pozo-principal-informe-diciembre-2019-96ecb1.doc",
                FileName = "pozo-principal-informe-diciembre-2019-96ecb1.doc",
                FilePath = "documentos/pozo-principal-informe-diciembre-2019-96ecb1.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("0b62879a-db5f-4cce-a775-7d74b5763cb3"),
                Slug = "pozo-principal-informe-enero-2021-0b6287",
                Title = "Pozo Principal - Informe Enero 2021",
                PublicationDate = DateTime.Parse("2021-01-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a enero de 2021.",
                FileSize = 69474,
                Url = "/documentos/pozo-principal-informe-enero-2021-0b6287.docx",
                FileName = "pozo-principal-informe-enero-2021-0b6287.docx",
                FilePath = "documentos/pozo-principal-informe-enero-2021-0b6287.docx",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("9029EB69-0E77-4CF6-8622-C7902D565745")
            },
            new ()
            {
                Id = Guid.Parse("d4c383ec-fe79-4f98-8e49-7bb7ea386316"),
                Slug = "pozo-principal-informe-febrero-2020-d4c383",
                Title = "Pozo Principal - Informe Febrero 2020",
                PublicationDate = DateTime.Parse("2020-02-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a febrero de 2020.",
                FileSize = 1082368,
                Url = "/documentos/pozo-principal-informe-febrero-2020-d4c383.doc",
                FileName = "pozo-principal-informe-febrero-2020-d4c383.doc",
                FilePath = "documentos/pozo-principal-informe-febrero-2020-d4c383.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("b040e2af-95cc-432d-8616-696f1a0ac2e4"),
                Slug = "pozo-principal-informe-julio-2017-con-cmara-b040e2",
                Title = "Pozo Principal - Informe Julio 2017 con Cámara",
                PublicationDate = DateTime.Parse("2017-07-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico del pozo principal con inspección realizada mediante cámara en julio de 2017.",
                FileSize = 68692,
                Url = "/documentos/pozo-principal-informe-julio-2017-con-cmara-b040e2.docx",
                FileName = "pozo-principal-informe-julio-2017-con-cmara-b040e2.docx",
                FilePath = "documentos/pozo-principal-informe-julio-2017-con-cmara-b040e2.docx",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("9029EB69-0E77-4CF6-8622-C7902D565745")
            },
            new ()
            {
                Id = Guid.Parse("276b8a7c-96ff-4a63-8c89-e7be4daee12b"),
                Slug = "pozo-principal-informe-julio-2017-276b8a",
                Title = "Pozo Principal - Informe Julio 2017",
                PublicationDate = DateTime.Parse("2017-07-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a julio de 2017.",
                FileSize = 1080320,
                Url = "/documentos/pozo-principal-informe-julio-2017-276b8a.doc",
                FileName = "pozo-principal-informe-julio-2017-276b8a.doc",
                FilePath = "documentos/pozo-principal-informe-julio-2017-276b8a.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("19ea3ee6-76f1-47fd-83b1-c7cd53979fc9"),
                Slug = "pozo-principal-informe-julio-2023-19ea3e",
                Title = "Pozo Principal - Informe Julio 2023",
                PublicationDate = DateTime.Parse("2023-07-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a julio de 2023.",
                FileSize = 2524044,
                Url = "/documentos/pozo-principal-informe-julio-2023-19ea3e.docx",
                FileName = "pozo-principal-informe-julio-2023-19ea3e.docx",
                FilePath = "documentos/pozo-principal-informe-julio-2023-19ea3e.docx",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("9029EB69-0E77-4CF6-8622-C7902D565745")
            },
            new ()
            {
                Id = Guid.Parse("6a20c483-c5d6-4c6f-8888-7dbc398c85e7"),
                Slug = "pozo-principal-informe-junio-2019-6a20c4",
                Title = "Pozo Principal - Informe Junio 2019",
                PublicationDate = DateTime.Parse("2019-06-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a junio de 2019.",
                FileSize = 1080832,
                Url = "/documentos/pozo-principal-informe-junio-2019-6a20c4.doc",
                FileName = "pozo-principal-informe-junio-2019-6a20c4.doc",
                FilePath = "documentos/pozo-principal-informe-junio-2019-6a20c4.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("aeedb763-c1f2-4cf7-bd47-2657b5167ea1"),
                Slug = "pozo-principal-informe-junio-2020-aeedb7",
                Title = "Pozo Principal - Informe Junio 2020",
                PublicationDate = DateTime.Parse("2020-06-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a junio de 2020.",
                FileSize = 1092096,
                Url = "/documentos/pozo-principal-informe-junio-2020-aeedb7.doc",
                FileName = "pozo-principal-informe-junio-2020-aeedb7.doc",
                FilePath = "documentos/pozo-principal-informe-junio-2020-aeedb7.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("18845405-8bcb-4895-b12a-5af31d2d9814"),
                Slug = "pozo-principal-informe-junio-2021-188454",
                Title = "Pozo Principal - Informe Junio 2021",
                PublicationDate = DateTime.Parse("2021-06-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a junio de 2021.",
                FileSize = 267832,
                Url = "/documentos/pozo-principal-informe-junio-2021-188454.docx",
                FileName = "pozo-principal-informe-junio-2021-188454.docx",
                FilePath = "documentos/pozo-principal-informe-junio-2021-188454.docx",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("9029EB69-0E77-4CF6-8622-C7902D565745")
            },
            new ()
            {
                Id = Guid.Parse("7ac7432d-3ea8-412f-818b-6f5f3ddaed01"),
                Slug = "pozo-principal-informe-marzo-2019-7ac743",
                Title = "Pozo Principal - Informe Marzo 2019",
                PublicationDate = DateTime.Parse("2019-03-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a marzo de 2019.",
                FileSize = 1080832,
                Url = "/documentos/pozo-principal-informe-marzo-2019-7ac743.doc",
                FileName = "pozo-principal-informe-marzo-2019-7ac743.doc",
                FilePath = "documentos/pozo-principal-informe-marzo-2019-7ac743.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("6c066e8c-00ad-4601-872b-61616435807c"),
                Slug = "pozo-principal-informe-noviembre-2020-6c066e",
                Title = "Pozo Principal - Informe Noviembre 2020",
                PublicationDate = DateTime.Parse("2020-11-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a noviembre de 2020.",
                FileSize = 1081856,
                Url = "/documentos/pozo-principal-informe-noviembre-2020-6c066e.doc",
                FileName = "pozo-principal-informe-noviembre-2020-6c066e.doc",
                FilePath = "documentos/pozo-principal-informe-noviembre-2020-6c066e.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("cce54b81-2b9e-4c8e-b67a-b2d33a1d3300"),
                Slug = "pozo-principal-informe-setiembre-2019-cce54b",
                Title = "Pozo Principal - Informe Setiembre 2019",
                PublicationDate = DateTime.Parse("2019-09-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a setiembre de 2019.",
                FileSize = 1092096,
                Url = "/documentos/pozo-principal-informe-setiembre-2019-cce54b.doc",
                FileName = "pozo-principal-informe-setiembre-2019-cce54b.doc",
                FilePath = "documentos/pozo-principal-informe-setiembre-2019-cce54b.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("c1418ee7-fec4-4f57-8858-be8b8b360174"),
                Slug = "pozo-principal-informe-setiembre-2020-c1418e",
                Title = "Pozo Principal - Informe Setiembre 2020",
                PublicationDate = DateTime.Parse("2020-09-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe técnico y operativo del pozo principal correspondiente a setiembre de 2020.",
                FileSize = 1081856,
                Url = "/documentos/pozo-principal-informe-setiembre-2020-c1418e.doc",
                FileName = "pozo-principal-informe-setiembre-2020-c1418e.doc",
                FilePath = "documentos/pozo-principal-informe-setiembre-2020-c1418e.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("43a31016-da6b-4135-9399-d335a64a1e07"),
                Slug = "pozo-principal-resultados-anlisis-de-agua-43a310",
                Title = "Pozo Principal - Resultados Análisis de Agua",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Resultados de análisis de calidad de agua realizados al pozo principal.",
                FileSize = 356966,
                Url = "/documentos/pozo-principal-resultados-anlisis-de-agua-43a310.pdf",
                FileName = "pozo-principal-resultados-anlisis-de-agua-43a310.pdf",
                FilePath = "documentos/pozo-principal-resultados-anlisis-de-agua-43a310.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("41bb7255-62cc-4fde-b89b-22004fa0e44f"),
                Slug = "reglamento-asadas-2020-41bb72",
                Title = "Reglamento Asadas 2020",
                PublicationDate = DateTime.Parse("2020-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Reglamento relacionado con la organización y funcionamiento de las ASADAS.",
                FileSize = 885268,
                Url = "/documentos/reglamento-asadas-2020-41bb72.pdf",
                FileName = "reglamento-asadas-2020-41bb72.pdf",
                FilePath = "documentos/reglamento-asadas-2020-41bb72.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("95389740-e900-44a6-80c3-9e78a64e6df5"),
                Slug = "reglamento-tcnico-prestacin-de-servicios-953897",
                Title = "Reglamento Técnico Prestación de Servicios",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Reglamento técnico para la prestación de servicios y gestión operativa institucional.",
                FileSize = 1697280,
                Url = "/documentos/reglamento-tcnico-prestacin-de-servicios-953897.doc",
                FileName = "reglamento-tcnico-prestacin-de-servicios-953897.doc",
                FilePath = "documentos/reglamento-tcnico-prestacin-de-servicios-953897.doc",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("1FA5211F-F4EA-42D1-8C5E-DEE71D9A75A7")
            },
            new ()
            {
                Id = Guid.Parse("988afba5-c1eb-4680-b5cf-4070942fedf7"),
                Slug = "rendicin-de-cuentas-informe-tesorera-2023-988afb",
                Title = "Rendición de Cuentas - Informe Tesorería 2023",
                PublicationDate = DateTime.Parse("2023-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Informe de rendición de cuentas y tesorería correspondiente al período 2023.",
                FileSize = 220726,
                Url = "/documentos/rendicin-de-cuentas-informe-tesorera-2023-988afb.pdf",
                FileName = "rendicin-de-cuentas-informe-tesorera-2023-988afb.pdf",
                FilePath = "documentos/rendicin-de-cuentas-informe-tesorera-2023-988afb.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("f195d8b1-0fc3-43ef-93df-b68765580542"),
                Slug = "resultados-revisin-hidrantes-bomberos-2014-f195d8",
                Title = "Resultados Revisión Hidrantes Bomberos 2014",
                PublicationDate = DateTime.Parse("2014-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Resultados de revisión y evaluación de hidrantes realizada por bomberos en 2014.",
                FileSize = 185961,
                Url = "/documentos/resultados-revisin-hidrantes-bomberos-2014-f195d8.pdf",
                FileName = "resultados-revisin-hidrantes-bomberos-2014-f195d8.pdf",
                FilePath = "documentos/resultados-revisin-hidrantes-bomberos-2014-f195d8.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("a09dff75-96bf-49be-8554-c4f5b9989df4"),
                Slug = "resultados-revisin-hidrantes-bomberos-2017-a09dff",
                Title = "Resultados Revisión Hidrantes Bomberos 2017",
                PublicationDate = DateTime.Parse("2017-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Resultados de revisión y evaluación de hidrantes realizada por bomberos en 2017.",
                FileSize = 122037,
                Url = "/documentos/resultados-revisin-hidrantes-bomberos-2017-a09dff.pdf",
                FileName = "resultados-revisin-hidrantes-bomberos-2017-a09dff.pdf",
                FilePath = "documentos/resultados-revisin-hidrantes-bomberos-2017-a09dff.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("4f29e2f3-b802-4001-bd41-eef6d7a26047"),
                Slug = "resultados-revisin-hidrantes-bomberos-2019-4f29e2",
                Title = "Resultados Revisión Hidrantes Bomberos 2019",
                PublicationDate = DateTime.Parse("2019-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Resultados de revisión y evaluación de hidrantes realizada por bomberos en 2019.",
                FileSize = 56508,
                Url = "/documentos/resultados-revisin-hidrantes-bomberos-2019-4f29e2.pdf",
                FileName = "resultados-revisin-hidrantes-bomberos-2019-4f29e2.pdf",
                FilePath = "documentos/resultados-revisin-hidrantes-bomberos-2019-4f29e2.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("4a6c5ce6-6463-49d5-9b86-d07a8b4f5a50"),
                Slug = "resultados-revisin-hidrantes-bomberos-2020-4a6c5c",
                Title = "Resultados Revisión Hidrantes Bomberos 2020",
                PublicationDate = DateTime.Parse("2020-12-01T00:00:00Z").ToUniversalTime(),
                Description = "Resultados de revisión y evaluación de hidrantes realizada por bomberos en 2020.",
                FileSize = 349913,
                Url = "/documentos/resultados-revisin-hidrantes-bomberos-2020-4a6c5c.pdf",
                FileName = "resultados-revisin-hidrantes-bomberos-2020-4a6c5c.pdf",
                FilePath = "documentos/resultados-revisin-hidrantes-bomberos-2020-4a6c5c.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("09282776-33e6-40e1-ab3d-c8160eb24c85"),
                Slug = "convenio-de-instalacin-de-medidores-adicionales-092827",
                Title = "Convenio de Instalación de Medidores Adicionales",
                PublicationDate = DateTime.Parse("2026-05-14T03:05:16.4598920Z").ToUniversalTime(),
                Description = "Documento para formalizar la instalación de medidores adicionales en una propiedad.",
                FileSize = 78263,
                Url = "/documentos/convenio-de-instalacin-de-medidores-adicionales-092827.pdf",
                FileName = "convenio-de-instalacin-de-medidores-adicionales-092827.pdf",
                FilePath = "documentos/convenio-de-instalacin-de-medidores-adicionales-092827.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("5385af33-451c-46b2-98de-b4594909eac8"),
                Slug = "machote-para-dudas-y-sugerencias-5385af",
                Title = "Machote para Dudas y Sugerencias",
                PublicationDate = DateTime.Parse("2026-05-14T03:05:19.7115915Z").ToUniversalTime(),
                Description = "Formulario para la presentación de dudas, consultas, sugerencias o comentarios por parte de los usuarios.",
                FileSize = 28921,
                Url = "/documentos/machote-para-dudas-y-sugerencias-5385af.pdf",
                FileName = "machote-para-dudas-y-sugerencias-5385af.pdf",
                FilePath = "documentos/machote-para-dudas-y-sugerencias-5385af.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("1fd75994-f97a-4baf-a7cd-2cf093bd5ce8"),
                Slug = "solicitud-para-cambio-de-nombre-en-recibo-de-agua-1fd759",
                Title = "Solicitud para Cambio de Nombre en Recibo de Agua",
                PublicationDate = DateTime.Parse("2026-05-14T03:05:11.9456748Z").ToUniversalTime(),
                Description = "Formulario para solicitar el cambio de titularidad en el recibo del servicio de agua.",
                FileSize = 28695,
                Url = "/documentos/solicitud-para-cambio-de-nombre-en-recibo-de-agua-1fd759.pdf",
                FileName = "solicitud-para-cambio-de-nombre-en-recibo-de-agua-1fd759.pdf",
                FilePath = "documentos/solicitud-para-cambio-de-nombre-en-recibo-de-agua-1fd759.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
            new ()
            {
                Id = Guid.Parse("9b64cc80-9d20-4467-b1e5-563be1c71c15"),
                Slug = "solicitud-para-derechos-de-agua-9b64cc",
                Title = "Solicitud para Derechos de Agua",
                PublicationDate = DateTime.Parse("2026-05-14T03:05:09.3767848Z").ToUniversalTime(),
                Description = "Formulario para solicitar derechos de conexión o acceso al servicio de agua potable.",
                FileSize = 148891,
                Url = "/documentos/solicitud-para-derechos-de-agua-9b64cc.pdf",
                FileName = "solicitud-para-derechos-de-agua-9b64cc.pdf",
                FilePath = "documentos/solicitud-para-derechos-de-agua-9b64cc.pdf",
                StatusId = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"),
                DocumentTypeId = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8")
            },
        };
    }
}