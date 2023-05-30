namespace School.SoftwareMakingProject.Domain.DbEntities
{
    public class User
    {
        public Guid? Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? TC_NO { get; set; }
        public string? Kullanici_Adi { get; set; }
        public string? Sifre { get; set; }
        public string? Telefon_No { get; set; }
        public string? e_mail { get; set; }
        public string? Adres { get; set; }
        public int? Kullanici_Tipi { get; set; }
    }
}
