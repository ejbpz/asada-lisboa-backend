using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.Seeders.SeedData
{
    /// <summary>
    /// Provides seed data for news.
    /// </summary>
    public static class NewsSeedData
    {
        /// <summary>
        /// Gets the list of seed news.
        /// </summary>  
        /// <param name="categories">The list of categories.</param>
        /// <returns>A list of seed news.</returns>
        public static List<New> Get(List<Category> categories)
        {
            var evento = categories.First(x => x.Id == Guid.Parse("A12D2AB5-24D2-420C-90D2-2CC3468A33F3"));
            var ahorro = categories.First(x => x.Id == Guid.Parse("88AF46BE-8E2E-4451-A746-EE0864E7145A"));
            var recibos = categories.First(x => x.Id == Guid.Parse("5DCB6BA5-0084-4420-8C7E-152DF55F8B0B"));
            var reciclaje = categories.First(x => x.Id == Guid.Parse("CC1F685C-356E-46EC-A027-3C570FDC6009"));
            var sinpeMovil = categories.First(x => x.Id == Guid.Parse("82BC31FD-3438-4B17-8216-A77C863BFB19"));
            var mantenimiento = categories.First(x => x.Id == Guid.Parse("BE75E73D-145E-41B1-B3AC-149ABC0ADE06"));
            var asambleaOrdinaria = categories.First(x => x.Id == Guid.Parse("712B5566-24BB-4681-9B7D-E636BDC2AB8E"));
            var asambleaExtraordinaria = categories.First(x => x.Id ==  Guid.Parse("C5D3B3B3-E69B-48D9-93A5-A4803AF196EB"));

            return new()
            {
                new()
                {
                    Id = Guid.Parse("f3fb2a39-6489-47d4-bccc-4e1204e53554"),
                    Slug = "invitacin-asamblea-general-f3fb2a",
                    Title = "Invitación Asamblea General",
                    Description = """<p><span></span></p><h2 class="text-[1.5rem] font-bold font-hepta-slab">Para nuestros estimados asociados:</h2><p></p><p>La Junta Administrativa del acueducto, tiene el agrado de invitar a todos (as) los asociados (propietarios) de la Urbanización Lisboa, a participar de la Asamblea General Ordinaria anual 2024; a realizarse el <b><i>sábado 16 de marzo 2024</i></b> en las instalaciones del parque contiguo a los tanques. En ella la Junta Directiva dará su informe de las labores correspondientes al periodo 2023. Acompáñenos e infórmese del acontecer de nuestra&nbsp;<span>ASADA.</span></p><p></p><ol><li>Primera convocatoria a la 1:00 pm</li><li>Segunda convocatoria a las 2:00 pm (<i>dará inicio con los propietarios que estén en el momento</i>). Al finalizar la Asamblea Ordinaria se procederá a realizar la Asamblea Extraordinaria con un único punto la elección de los puestos de vicepresidencia y tesorería.</li></ol><p></p><p><br></p><p>Ven y comparte con nosotros un cafecito.</p><p>Se rifarán canastas.</p><p><span class="text-[#737373] text-[0.85rem] font-light font-poppins"><br></span></p><p><span class="text-[#737373] text-[0.85rem] font-light font-poppins">Entrega de números de&nbsp;<span>1:00 PM A 2:00 PM</span></span><span></span></p>""",
                    ImageUrl = "/noticias/invitacin-asamblea-general-f3fb2a.jpg",
                    PublicationDate = DateTime.Parse("2023-03-05T00:00:00Z").ToUniversalTime(),
                    LastEditionDate = DateTime.Parse("2024-03-08T00:00:00Z").ToUniversalTime(),
                    FileName = "invitacin-asamblea-general-f3fb2a.jpg",
                    FilePath = "noticias/invitacin-asamblea-general-f3fb2a.jpg",
                    StatusId = Guid.Parse("5c1cebda-fc8c-44ac-997c-aaf015572d46"),
                    Categories = new List<Category>
                    {
                        evento,
                        asambleaOrdinaria,
                        asambleaExtraordinaria,
                    }
                },
                new()
                {
                    Id = Guid.Parse("ee181128-d03b-4e03-8d18-d1823d656e2c"),
                    Slug = "pagos-por-sinpe-ee1811",
                    Title = "Pagos por Sinpe",
                    Description = """<p><span></span></p><h2 class="text-[1.5rem] font-bold font-hepta-slab">Pague sus recibor por medio de Sinpe</h2><p></p><p></p><ol><li>SOLICITE AL 8459-5494 EL MONTO A CANCELAR. O A LA PÁGINA&nbsp;<a href="https://asadalisboa.org/recibos" target="_blank" rel="noopener noreferrer">https://asadalisboa.org/recibos</a>.</li><li>REALICE SU PAGO POR MEDIO DE SINPE MÓVIL AL NÚMERO 8459-5494 (A NOMBRE ASADA LISBOA).</li><li>EN EL DETALLE COLOCAR EL NÚMERO DE PAJA.</li><li>ENVIAR COMPROBANTE AL WHATSAPP Y ESPERAR LA CANCELACIÓN DEL RECIBO.</li></ol><p></p><p><br></p><p><b>NOTA:</b> DE NO ENVIAR EL COMPROBANTE CON SU MONTO EXACTO SU RECIBO AÚN ESTARÁ&nbsp;<span>PENDIENTE.</span></p><p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</p>""",
                    ImageUrl = "/noticias/pagos-por-sinpe-ee1811.jpg",
                    PublicationDate = DateTime.Parse("2023-03-05T00:00:00Z").ToUniversalTime(),
                    LastEditionDate = DateTime.Parse("2023-03-05T00:00:00Z").ToUniversalTime(),
                    FileName = "pagos-por-sinpe-ee1811.jpg",
                    FilePath = "noticias/pagos-por-sinpe-ee1811.jpg",
                    StatusId = Guid.Parse("5c1cebda-fc8c-44ac-997c-aaf015572d46"),
                    Categories = new List<Category>
                    {
                        recibos,
                        sinpeMovil,
                    }
                },
                new()
                {
                    Id = Guid.Parse("9e93563c-1f32-41ac-a9dd-c1b8fc70c906"),
                    Slug = "cancele-sus-recibos-con-sinpe-9e9356",
                    Title = "Cancele sus recibos con SINPE",
                    Description = """<p><span></span></p><h2 class="text-[1.5rem] font-bold font-hepta-slab">¡Cancele su recibo de agua por medio de SINPE!</h2><p>Realizar sus pagos por medio de SINPE Móvil, siguiendo estos pasos:</p><p></p><p></p><ol><li>Línea telefónica:&nbsp;<span>8459-5494.</span></li><li>Indique en el detalle&nbsp;<span>el número de&nbsp;</span><span>paja.</span></li><li>Envíe su comprobante al whatsapp.</li><li>RECIBO: En un&nbsp;<span>plazo de 24&nbsp;</span><span>horas hábiles&nbsp;</span><span>será emitido.</span></li></ol><p></p><p><br></p><p></p><h4 class="text-[1.1rem] font-normal font-hepta-slab">¡ATENCIÓN!</h4><p></p><p></p><p class="text-[1rem] font-light font-poppins">Si tiene mas de una factura pendiente o debe cancelar la reconexión puede sumar todos los montos y hacer una única transferencia.</p><p></p><p><br></p><p><span class="text-[#737373] text-[0.85rem] font-light font-poppins">RECUERDE ACTUALIZAR SUS DATOS AL 8459-5494 0 2433-3882 PARA RECIBIR SU FACTURACION.</span></p>""",
                    ImageUrl = "/noticias/cancele-sus-recibos-con-sinpe-9e9356.jpg",
                    PublicationDate = DateTime.Parse("2023-03-05T00:00:00Z").ToUniversalTime(),
                    LastEditionDate = DateTime.Parse("2023-03-05T00:00:00Z").ToUniversalTime(),
                    FileName = "cancele-sus-recibos-con-sinpe-9e9356.jpg",
                    FilePath = "noticias/cancele-sus-recibos-con-sinpe-9e9356.jpg",
                    StatusId = Guid.Parse("5c1cebda-fc8c-44ac-997c-aaf015572d46"),
                    Categories = new List<Category>
                    {
                        recibos,
                        sinpeMovil,
                    }
                },
                new()
                {
                    Id = Guid.Parse("40028c20-85d4-475d-8028-5720a44c7b13"),
                    Slug = "importancia-del-reciclaje-de-papel-40028c",
                    Title = "Importancia del reciclaje de papel",
                    Description = """<p></p><h2 class="text-[1.5rem] font-bold font-hepta-slab">Para nuestros abonados:</h2><p></p><p>Aquí te presentamos algunas razones importantes por las que es crucial reciclar papel y también evitar imprimir innecesariamente:</p><p></p><ul><li><b>Conservación de Bosques:</b> La producción de papel está estrechamente relacionada con la tala de árboles. Al reciclar papel, contribuyes directamente a ayudar a recuperar las masas forestales de nuestro planeta.</li><li><b>Reducción de Plantaciones Comerciales:</b> En muchos lugares, los bosques naturales y otros ecosistemas son sustituidos por plantaciones de árboles de crecimiento rápido para la industria papelera. Esto ocurre principalmente por motivos económicos.</li><li><b>Menos Residuos en Vertederos:</b> El papel representa aproximadamente el 18% de los residuos que generamos cada día. En algunos países, cerca del 40% de los artículos hechos de papel y cartón terminan en vertederos.</li><li><b>Ahorro de Energía:</b> Fabricar papel reciclado a partir de papel usado permite ahorrar hasta un 60% de la energía empleada para producir papel directamente de la celulosa.</li><li><b>Preservación de Árboles y Vida Silvestre: </b>Una tonelada de papel reciclado equivale a salvar la vida de 17 árboles adultos. Además, al reciclar papel, se reduce la necesidad de talar más árboles, lo que beneficia a la biodiversidad y a las especies animales y vegetales.</li><li><b>Mejora de la Calidad del Aire:</b> Al reducir la cantidad de papel que se quema o descompone en vertederos, se disminuyen las emisiones de gases contaminantes, lo que a su vez mejora la calidad del aire que respiramos.</li><li><b>Conciencia y Responsabilidad:</b> Fomentar el reciclaje de papel también incentiva la conciencia ambiental y la responsabilidad en las personas.</li></ul><p></p><p><br></p><p>Por lo anterior en nuestra próxima asamblea del 16 de marzo. <b>NO</b> estaremos proporcionando a los asistentes informes impresos; Sin embargo, si alguien los requiere los puede solicitar en formato digital o impreso a nuestro Whatsapp (8459-5494) o lo puede descargar directamente desde nuestra página web: <a rel="noopener noreferrer" target="_blank">www.asadalisboa.org</a></p><p><br></p><p><span class="text-[#737373] text-[0.85rem] font-light font-poppins">"Cuidar el agua es cuidar nuestro futuro"</span></p>""",
                    ImageUrl = "/noticias/importancia-del-reciclaje-de-papel-40028c.jpg",
                    PublicationDate = DateTime.Parse("2024-03-11T00:00:00Z").ToUniversalTime(),
                    LastEditionDate = DateTime.Parse("2024-03-11T00:00:00Z").ToUniversalTime(),
                    FileName = "importancia-del-reciclaje-de-papel-40028c.jpg",
                    FilePath = "noticias/importancia-del-reciclaje-de-papel-40028c.jpg",
                    StatusId = Guid.Parse("5c1cebda-fc8c-44ac-997c-aaf015572d46"),
                    Categories = new List<Category>
                    {
                        reciclaje,
                        mantenimiento,
                    }
                },
                new()
                {
                    Id = Guid.Parse("27e3921a-6e91-40d3-a783-9fde56374d93"),
                    Slug = "indicacin-a-asociados-27e392",
                    Title = "Indicación a asociados",
                    Description = """<h2 class="text-[1.5rem] font-bold font-hepta-slab">Estimados asociados a la ASADA de Urbanización Lisboa.</h2><h2 class="text-[1.5rem] font-bold font-hepta-slab"><p></p><p class="text-[1rem] font-light font-poppins">Se les informa que por motivos de espacio, se le solicitará que solo asista el <span>propietario registral</span> a la <span>Asamblea Ordinaria y Extraordinaria </span>que se llevará a cabo el 16 de marzo a la 1:00 p.m., favor traer su cédula.</p><p class="text-[1rem] font-light font-poppins"><br></p><p></p><p class="text-[1rem] font-light font-poppins"><span class="text-[#737373] text-[0.85rem] font-light font-poppins">Si otra persona lo va a representar, deberá traer una autorización escrita y la copia de ambas cédular.</span></p><p class="text-[1rem] font-light font-poppins"><span class="text-[#737373] text-[0.85rem] font-light font-poppins"><br></span></p></h2><h4 class="text-[1.1rem] font-normal font-hepta-slab">Ejemplo de machote:</h4><span>Yo: ******,</span><br><span>Cédula: ******,</span><br><span>Propietario del lote: ******,</span><br><span>Autorizo a: *******, cédula: *******&nbsp; para que en mi nombre y representación participe en la Asamblea de la ASADA.</span><br><span>Fecha: ************.</span><br><span>Carta firmada por ambos.</span><br><p><span></span></p><p><span><br></span></p>""",
                    ImageUrl = "/noticias/indicacin-a-asociados-27e392.jpg",
                    PublicationDate = DateTime.Parse("2024-03-08T00:00:00Z").ToUniversalTime(),
                    LastEditionDate = DateTime.Parse("2024-03-08T00:00:00Z").ToUniversalTime(),
                    FileName = "indicacin-a-asociados-27e392.jpg",
                    FilePath = "noticias/indicacin-a-asociados-27e392.jpg",
                    StatusId = Guid.Parse("5c1cebda-fc8c-44ac-997c-aaf015572d46"),
                    Categories = new List<Category>
                    {
                        asambleaOrdinaria,
                        asambleaExtraordinaria,
                    }
                },
                new()
                {
                    Id = Guid.Parse("db88df36-7bdb-4668-bef0-80a518a9f908"),
                    Slug = "pequeas-acciones-para-ahorrar-agua-en-casa-db88df",
                    Title = "Pequeñas acciones para ahorrar agua en casa",
                    Description = """<h2 class="text-[1.5rem] font-bold font-hepta-slab">Día mundial del agua</h2><p>El día 22 de marzo es asignado para indicar el día mundial de agua y te traemos pequeñas acciones para ahorrar agua en casa:</p><p></p><ul><li>Tomar duchas más cortas.</li><li>Reparar las fugas de agua en tu hogar.</li><li>Esperar hasta tener suficiente ropa para lavar.</li><li>Lavar su auto con cubeta, usando agua de lluvia.</li><li>Regar las plantas temprano en la mañana o en la noche.</li></ul><p><br></p><p>Estas siendo unas cuántas indicaciones para tener conciencia sobre nuestro consumo de agua.</p><p><br></p><p>"Sabemos que cuando protegemos nuestros océanos, protegemos nuestro futuro".</p><p></p>""",
                    ImageUrl = "/noticias/pequeas-acciones-para-ahorrar-agua-en-casa-db88df.jpeg",
                    PublicationDate = DateTime.Parse("2024-03-23T00:00:00Z").ToUniversalTime(),
                    LastEditionDate = DateTime.Parse("2024-03-23T00:00:00Z").ToUniversalTime(),
                    FileName = "pequeas-acciones-para-ahorrar-agua-en-casa-db88df.jpeg",
                    FilePath = "noticias/pequeas-acciones-para-ahorrar-agua-en-casa-db88df.jpeg",
                    StatusId = Guid.Parse("5c1cebda-fc8c-44ac-997c-aaf015572d46"),
                    Categories = new List<Category>
                    {
                        ahorro,
                        reciclaje,
                        mantenimiento,
                    }
                }
            };
        }
    }
}
