namespace Cars.Parts.Toyota.ProaceVerso
{
    using System;
    using Cars.I18n;

    public static class ToyotaProaceVersoParts
    {
        public static readonly Action<PartBuilder> Seats8 = p => p.PartNumber("SITZE_8").Abstract(a => a.GermanGermany("8 Sitze (compact: 5 Sitze)"));

        public static readonly Action<PartBuilder> Seats9 = p => p.PartNumber("SITZE_9").Abstract(a => a.GermanGermany("9 Sitze (compact: 6 Sitze)"));

        public static readonly Action<PartBuilder> PassengerBench = p => p.PartNumber("DOPPELSITZBANK_VORNE").Abstract(a => a.GermanGermany("Beifahrerdoppelsitzbank"));

        public static readonly Action<PartBuilder> IndividualSeats = p => p.PartNumber("EINZELSITZE_VORNE").Abstract(a => a.GermanGermany("Einzelsitze vorne"));

        public static readonly Action<PartBuilder> RemovableSeats = p => p.PartNumber("SITZE_INDIV").Abstract(a => a.GermanGermany("Sitze individuell verschieb- und herausnehmbar"));

        public static readonly Action<PartBuilder> SeatHeating = p => p.PartNumber("SITZHEIZUNG").Abstract(a => a.GermanGermany("Sitzheizung Fahrer und Beifahrer"));

        public static readonly Action<PartBuilder> SlidingDoorPassenger = p => p.PartNumber("SCHIEBETUER_BEIFAHRER").Abstract(a => a.GermanGermany("Schiebetür Beifahrerseite"));

        public static readonly Action<PartBuilder> SlidingDoorDriver = p => p.PartNumber("SCHIEBETUER_FAHRER").Abstract(a => a.GermanGermany("zusätzliche Schiebetür Fahrerseite"));

        public static readonly Action<PartBuilder> RearDoorWithWindows = p => p.PartNumber("HECKTUER_FENSTER").Abstract(a => a.GermanGermany("Hecktüren mit Fenster"));

        public static readonly Action<PartBuilder> RearWindow = p => p.PartNumber("HECKSCHEIBE_FENSTER").Abstract(a => a.GermanGermany("Heckklappe mit separat zu öffnender Heckscheibe"));

        public static readonly Action<PartBuilder> Airbags = p => p.PartNumber("AIRBAGS").Abstract(a => a.GermanGermany("Fahrer- und Beifahrerairbag/Seitenairbags"));

        public static readonly Action<PartBuilder> FogLight = p => p.PartNumber("NEBELSCHEINWERFER").Abstract(a => a.GermanGermany("Nebelscheinwerfer und Tagfahrlicht"));

        public static readonly Action<PartBuilder> HeadLightCleaner = p => p.PartNumber("SCHEINWERFER_REINIGUNG").Abstract(a => a.GermanGermany("Scheinwerfer-Reinigungsanlage"));

        public static readonly Action<PartBuilder> CruiseControl = p => p.PartNumber("TEMPOMAT").Abstract(a => a.GermanGermany("Geschwindigkeitsregelanlage"));

        public static readonly Action<PartBuilder> ParkingSensors = p => p.PartNumber("PARKSENSOREN").Abstract(a => a.GermanGermany("Parksensoren vorne und hinten"));

        public static readonly Action<PartBuilder> PowerMirror = p => p.PartNumber("SPIEGEL_ELEKTRISCH").Abstract(a => a.GermanGermany("Außenspiegel elektrisch einstellbar und beheizbar"));

        public static readonly Action<PartBuilder> PowerMirrorFoldIn = p => p.PartNumber("SPIEGEL_ANKLAPPBAR").Abstract(a => a.GermanGermany("Außenspiegel elektrisch anklappbar"));

        public static readonly Action<PartBuilder> AirConditionAutomatic = p => p.PartNumber("KLIMA_AUTOMATIK").Abstract(a => a.GermanGermany("Klimaautomatik"));

        public static readonly Action<PartBuilder> Radio = p => p.PartNumber("RADIO").Abstract(a => a.GermanGermany("Radio/USB und 4 Lautsprecher"));

        public static readonly Action<PartBuilder> PowerDoorLocks = p => p.PartNumber("ZENTRALVERRIEGELUNG").Abstract(a => a.GermanGermany("Zentralverriegelung mit Funkfernbedienung"));

        public static readonly Action<PartBuilder> SmarkKey = p => p.PartNumber("SMARTKEY").Abstract(a => a.GermanGermany("Smart-Key-System"));

        public static readonly Action<PartBuilder> RearViewCamera = p => p.PartNumber("CAM_REAR").Abstract(a => a.GermanGermany("Rückfahrkamera und 7\"-Display"));

        public static readonly Action<PartBuilder> FoldingTables = p => p.PartNumber("TABLE").Abstract(a => a.GermanGermany("Klapptische an den Rückseiten der Vordersitze"));

        public static readonly Action<PartBuilder> AlloyWheel17 = p => p.PartNumber("17_ALU").Abstract(a => a.GermanGermany("17\"-Leichtmetallfelgen"));

        public static readonly Action<PartBuilder> SteelWheel16 = p => p.PartNumber("16_STEEL").Abstract(a => a.GermanGermany("16\"-Stahlräder"));

        public static readonly Action<PartBuilder> SteelWheel17 = p => p.PartNumber("17_STEEL").Abstract(a => a.GermanGermany("17\"-Stahlräder"));
    }
}