namespace ReservationAPI.Application
{
    public static class Const
    {
        public static string DateFormat => "yyyy-MM-dd";
        public static string DateWithouFormat=> "La fecha no está en el formato correcto";
        public static string TimeSeparator => ":";
        public static string DateBeforeNow => "Fecha incongruente";
        public static string DateDisable => "Ya existe una reserva para esta fecha y hora";
        public static string MoreOneVisit => "No puede tener màs de un turno por dìa";
    }
}
