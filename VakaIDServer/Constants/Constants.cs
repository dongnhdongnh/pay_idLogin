using System.Collections.Generic;
using VakaxaIDServer.Models;

namespace VakaxaIDServer.Constants
{
    public static class Constants
    {
        public static Dictionary<string, string> ListTimeZone = new Dictionary<string, string>
        {
            {"American Samoa", "(GMT-11:00) American Samoa"},
            {"International Date Line West", "(GMT-11:00) International Date Line West"},
            {"Midway Island", "(GMT-11:00) Midway Island"},
            {"Hawaii", "(GMT-10:00) Hawaii"},
            {"Alaska", "(GMT-09:00) Alaska"},
            {"Pacific Time (US & Canada)", "(GMT-08:00) Pacific Time (US & Canada)"},
            {"Tijuana", "(GMT-08:00) Tijuana"},
            {"Arizona", "(GMT-07:00) Arizona"},
            {"Chihuahua", "(GMT-07:00) Chihuahua"},
            {"Mazatlan", "(GMT-07:00) Mazatlan"},
            {"Mountain Time (US & Canada)", "(GMT-07:00) Mountain Time (US & Canada)"},
            {"Central America", "(GMT-06:00) Central America"},
            {"Central Time (US & Canada)", "(GMT-06:00) Central Time (US & Canada)"},
            {"Guadalajara", "(GMT-06:00) Guadalajara"},
            {"Mexico City", "(GMT-06:00) Mexico City"},
            {"Monterrey", "(GMT-06:00) Monterrey"},
            {"Saskatchewan", "(GMT-06:00) Saskatchewan"},
            {"Bogota", "(GMT-05:00) Bogota"},
            {"Eastern Time (US & Canada)", "(GMT-05:00) Eastern Time (US & Canada)"},
            {"Indiana (East)", "(GMT-05:00) Indiana (East)"},
            {"Lima", "(GMT-05:00) Lima"},
            {"Quito", "(GMT-05:00) Quito"},
            {"Atlantic Time (Canada)", "(GMT-04:00) Atlantic Time (Canada)"},
            {"Caracas", "(GMT-04:00) Caracas"},
            {"Georgetown", "(GMT-04:00) Georgetown"},
            {"La Paz", "(GMT-04:00) La Paz"},
            {"Santiago", "(GMT-04:00) Santiago"},
            {"Newfoundland", "(GMT-03:30) Newfoundland"},
            {"Brasilia", "(GMT-03:00) Brasilia"},
            {"Buenos Aires", "(GMT-03:00) Buenos Aires"},
            {"Greenland", "(GMT-03:00) Greenland"},
            {"Montevideo", "(GMT-03:00) Montevideo"},
            {"Mid-Atlantic", "(GMT-02:00) Mid-Atlantic"},
            {"Azores", "(GMT-01:00) Azores"},
            {"Cape Verde Is.", "(GMT-01:00) Cape Verde Is."},
            {"Casablanca", "(GMT+00:00) Casablanca"},
            {"Edinburgh", "(GMT+00:00) Edinburgh"},
            {"Lisbon", "(GMT+00:00) Lisbon"},
            {"London", "(GMT+00:00) London"},
            {"Monrovia", "(GMT+00:00) Monrovia"},
            {"UTC", "(GMT+00:00) UTC"},
            {"Amsterdam", "(GMT+01:00) Amsterdam"},
            {"Belgrade", "(GMT+01:00) Belgrade"},
            {"Berlin", "(GMT+01:00) Berlin"},
            {"Bern", "(GMT+01:00) Bern"},
            {"Bratislava", "(GMT+01:00) Bratislava"},
            {"Brussels", "(GMT+01:00) Brussels"},
            {"Budapest", "(GMT+01:00) Budapest"},
            {"Copenhagen", "(GMT+01:00) Copenhagen"},
            {"Dublin", "(GMT+01:00) Dublin"},
            {"Europe/Berlin", "(GMT+01:00) Europe/Berlin"},
            {"Ljubljana", "(GMT+01:00) Ljubljana"},
            {"Madrid", "(GMT+01:00) Madrid"},
            {"Paris", "(GMT+01:00) Paris"},
            {"Prague", "(GMT+01:00) Prague"},
            {"Rome", "(GMT+01:00) Rome"},
            {"Sarajevo", "(GMT+01:00) Sarajevo"},
            {"Skopje", "(GMT+01:00) Skopje"},
            {"Stockholm", "(GMT+01:00) Stockholm"},
            {"Vienna", "(GMT+01:00) Vienna"},
            {"Warsaw", "(GMT+01:00) Warsaw"},
            {"West Central Africa", "(GMT+01:00) West Central Africa"},
            {"Zagreb", "(GMT+01:00) Zagreb"},
            {"Zurich", "(GMT+01:00) Zurich"},
            {"Athens", "(GMT+02:00) Athens"},
            {"Bucharest", "(GMT+02:00) Bucharest"},
            {"Cairo", "(GMT+02:00) Cairo"},
            {"Harare", "(GMT+02:00) Harare"},
            {"Helsinki", "(GMT+02:00) Helsinki"},
            {"Jerusalem", "(GMT+02:00) Jerusalem"},
            {"Kaliningrad", "(GMT+02:00) Kaliningrad"},
            {"Kyiv", "(GMT+02:00) Kyiv"},
            {"Pretoria", "(GMT+02:00) Pretoria"},
            {"Riga", "(GMT+02:00) Riga"},
            {"Sofia", "(GMT+02:00) Sofia"},
            {"Tallinn", "(GMT+02:00) Tallinn"},
            {"Vilnius", "(GMT+02:00) Vilnius"},
            {"Baghdad", "(GMT+03:00) Baghdad"},
            {"Istanbul", "(GMT+03:00) Istanbul"},
            {"Kuwait", "(GMT+03:00) Kuwait"},
            {"Minsk", "(GMT+03:00) Minsk"},
            {"Moscow", "(GMT+03:00) Moscow"},
            {"Nairobi", "(GMT+03:00) Nairobi"},
            {"Riyadh", "(GMT+03:00) Riyadh"},
            {"St. Petersburg", "(GMT+03:00) St. Petersburg"},
            {"Volgograd", "(GMT+03:00) Volgograd"},
            {"Tehran", "(GMT+03:30) Tehran"},
            {"Abu Dhabi", "(GMT+04:00) Abu Dhabi"},
            {"Baku", "(GMT+04:00) Baku"},
            {"Muscat", "(GMT+04:00) Muscat"},
            {"Samara", "(GMT+04:00) Samara"},
            {"Tbilisi", "(GMT+04:00) Tbilisi"},
            {"Yerevan", "(GMT+04:00) Yerevan"},
            {"Kabul", "(GMT+04:30) Kabul"},
            {"Ekaterinburg", "(GMT+05:00) Ekaterinburg"},
            {"Islamabad", "(GMT+05:00) Islamabad"},
            {"Karachi", "(GMT+05:00) Karachi"},
            {"Tashkent", "(GMT+05:00) Tashkent"},
            {"Chennai", "(GMT+05:30) Chennai"},
            {"Kolkata", "(GMT+05:30) Kolkata"},
            {"Mumbai", "(GMT+05:30) Mumbai"},
            {"New Delhi", "(GMT+05:30) New Delhi"},
            {"Sri Jayawardenepura", "(GMT+05:30) Sri Jayawardenepura"},
            {"Kathmandu", "(GMT+05:45) Kathmandu"},
            {"Almaty", "(GMT+06:00) Almaty"},
            {"Astana", "(GMT+06:00) Astana"},
            {"Dhaka", "(GMT+06:00) Dhaka"},
            {"Urumqi", "(GMT+06:00) Urumqi"},
            {"Rangoon", "(GMT+06:30) Rangoon"},
            {"Bangkok", "(GMT+07:00) Bangkok"},
            {"Hanoi", "(GMT+07:00) Hanoi"},
            {"Jakarta", "(GMT+07:00) Jakarta"},
            {"Krasnoyarsk", "(GMT+07:00) Krasnoyarsk"},
            {"Novosibirsk", "(GMT+07:00) Novosibirsk"},
            {"Beijing", "(GMT+08:00) Beijing"},
            {"Chongqing", "(GMT+08:00) Chongqing"},
            {"Hong Kong", "(GMT+08:00) Hong Kong"},
            {"Irkutsk", "(GMT+08:00) Irkutsk"},
            {"Kuala Lumpur", "(GMT+08:00) Kuala Lumpur"},
            {"Perth", "(GMT+08:00) Perth"},
            {"Singapore", "(GMT+08:00) Singapore"},
            {"Taipei", "(GMT+08:00) Taipei"},
            {"Ulaanbaatar", "(GMT+08:00) Ulaanbaatar"},
            {"Osaka", "(GMT+09:00) Osaka"},
            {"Sapporo", "(GMT+09:00) Sapporo"},
            {"Seoul", "(GMT+09:00) Seoul"},
            {"Tokyo", "(GMT+09:00) Tokyo"},
            {"Yakutsk", "(GMT+09:00) Yakutsk"},
            {"Adelaide", "(GMT+09:30) Adelaide"},
            {"Darwin", "(GMT+09:30) Darwin"},
            {"Brisbane", "(GMT+10:00) Brisbane"},
            {"Canberra", "(GMT+10:00) Canberra"},
            {"Guam", "(GMT+10:00) Guam"},
            {"Hobart", "(GMT+10:00) Hobart"},
            {"Melbourne", "(GMT+10:00) Melbourne"},
            {"Port Moresby", "(GMT+10:00) Port Moresby"},
            {"Sydney", "(GMT+10:00) Sydney"},
            {"Vladivostok", "(GMT+10:00) Vladivostok"},
            {"Magadan", "(GMT+11:00) Magadan"},
            {"New Caledonia", "(GMT+11:00) New Caledonia"},
            {"Solomon Is.", "(GMT+11:00) Solomon Is."},
            {"Srednekolymsk", "(GMT+11:00) Srednekolymsk"},
            {"Auckland", "(GMT+12:00) Auckland"},
            {"Fiji", "(GMT+12:00) Fiji"},
            {"Kamchatka", "(GMT+12:00) Kamchatka"},
            {"Marshall Is.", "(GMT+12:00) Marshall Is."},
            {"Wellington", "(GMT+12:00) Wellington"},
            {"Chatham Is.", "(GMT+12:45) Chatham Is."},
            {"Nuku'alofa", "(GMT+13:00) Nuku'alofa"},
            {"Samoa", "(GMT+13:00) Samoa"},
            {"Tokelau Is.", "(GMT+13:00) Tokelau Is."}
        };


        public static readonly Dictionary<string, string> IconLog = new Dictionary<string, string>
        {
            {LogAction.Avatar, "fa fa-user"},
            {LogAction.Login, "fa fa-user"},
            {LogAction.Logout, "fa fa-user"},
            {LogAction.UpdateProfile, "a-user"}
        };

        public static Dictionary<string, string> ListCurrency = new Dictionary<string, string>
        {
            {
                "USD",
                "United States Dollar (USD)"
            },
            {
                "EUR",
                "Euro (EUR)"
            },
            {
                "CNY",
                "Chinese Renminbi Yuan (CNY)"
            },
            {
                "GBP",
                "British Pound (GBP)"
            },
            {
                "CAD",
                "Canadian Dollar (CAD)"
            },
            {
                "AFN",
                "Afghan Afghani (AFN)"
            },
            {
                "ALL",
                "Albanian Lek (ALL)"
            },
            {
                "DZD",
                "Algerian Dinar (DZD)"
            },
            {
                "ARS",
                "Argentine Peso (ARS)"
            },
            {
                "AMD",
                "Armenian Dram (AMD)"
            },
            {
                "AWG",
                "Aruban Florin (AWG)"
            },
            {
                "AOA",
                "Aurora (AOA)"
            },
            {
                "AUD",
                "Australian Dollar (AUD)"
            },
            {
                "AZN",
                "Azerbaijani Manat (AZN)"
            },
            {
                "BSD",
                "Bahamian Dollar (BSD)"
            },
            {
                "BHD",
                "Bahraini Dinar (BHD)"
            },
            {
                "BDT",
                "Bangladeshi Taka (BDT)"
            },
            {
                "BBD",
                "Barbadian Dollar (BBD)"
            },
            {
                "BYN",
                "Belarusian Ruble (BYN)"
            },
            {
                "BYR",
                "Belarusian Ruble (BYR)"
            },
            {
                "BZD",
                "Belize Dollar (BZD)"
            },
            {
                "BMD",
                "Bermudian Dollar (BMD)"
            },
            {
                "BTN",
                "Bhutanese Ngultrum (BTN)"
            },
            {
                "BOB",
                "Bolivian Boliviano (BOB)"
            },
            {
                "BAM",
                "Bosnia and Herzegovina Convertible Mark (BAM)"
            },
            {
                "BWP",
                "Botswana Pula (BWP)"
            },
            {
                "BRL",
                "Brazilian Real (BRL)"
            },
            {
                "BND",
                "Brunei Dollar (BND)"
            },
            {
                "BGN",
                "Bulgarian Lev (BGN)"
            },
            {
                "BIF",
                "Burundian Franc (BIF)"
            },
            {
                "KHR",
                "Cambodian Riel (KHR)"
            },
            {
                "CVE",
                "Cape Verdean Escudo (CVE)"
            },
            {
                "KYD",
                "Cayman Islands Dollar (KYD)"
            },
            {
                "XAF",
                "Central African Cfa Franc (XAF)"
            },
            {
                "XPF",
                "Cfp Franc (XPF)"
            },
            {
                "CLP",
                "Chilean Peso (CLP)"
            },
            {
                "CNH",
                "Chinese Renminbi Yuan Offshore (CNH)"
            },
            {
                "COP",
                "Colombian Peso (COP)"
            },
            {
                "KMF",
                "Comorian Franc (KMF)"
            },
            {
                "CDF",
                "Congolese Franc (CDF)"
            },
            {
                "CRC",
                "Costa Rican Colón (CRC)"
            },
            {
                "HRK",
                "Croatian Kuna (HRK)"
            },
            {
                "CUC",
                "Cuban Convertible Peso (CUC)"
            },
            {
                "CZK",
                "Czech Koruna (CZK)"
            },
            {
                "DKK",
                "Danish Krone (DKK)"
            },
            {
                "DJF",
                "Djiboutian Franc (DJF)"
            },
            {
                "DOP",
                "Dominican Peso (DOP)"
            },
            {
                "XCD",
                "East Caribbean Dollar (XCD)"
            },
            {
                "EGP",
                "Egyptian Pound (EGP)"
            },
            {
                "ERN",
                "Eritrean Nakfa (ERN)"
            },
            {
                "EEK",
                "Estonian Kroon (EEK)"
            },
            {
                "ETB",
                "Ethiopian Birr (ETB)"
            },
            {
                "FKP",
                "Falkland Pound (FKP)"
            },
            {
                "FJD",
                "Fijian Dollar (FJD)"
            },
            {
                "GMD",
                "Gambian Dalasi (GMD)"
            },
            {
                "GEL",
                "Georgian Lari (GEL)"
            },
            {
                "GHS",
                "Ghanaian Cedi (GHS)"
            },
            {
                "GIP",
                "Gibraltar Pound (GIP)"
            },
            {
                "XAU",
                "Gold (Troy Ounce) (XAU)"
            },
            {
                "GTQ",
                "Guatemalan Quetzal (GTQ)"
            },
            {
                "GGP",
                "Guernsey Pound (GGP)"
            },
            {
                "GNF",
                "Guinean Franc (GNF)"
            },
            {
                "GYD",
                "Guyanese Dollar (GYD)"
            },
            {
                "HTG",
                "Haitian Gourde (HTG)"
            },
            {
                "HNL",
                "Honduran Lempira (HNL)"
            },
            {
                "HKD",
                "Hong Kong Dollar (HKD)"
            },
            {
                "HUF",
                "Hungarian Forint (HUF)"
            },
            {
                "ISK",
                "Icelandic Króna (ISK)"
            },
            {
                "INR",
                "Indian Rupee (INR)"
            },
            {
                "IDR",
                "Indonesian Rupiah (IDR)"
            },
            {
                "IQD",
                "Iraqi Dinar (IQD)"
            },
            {
                "IMP",
                "Isle of Man Pound (IMP)"
            },
            {
                "ILS",
                "Israeli New Sheqel (ILS)"
            },
            {
                "JMD",
                "Jamaican Dollar (JMD)"
            },
            {
                "JPY",
                "Japanese Yen (JPY)"
            },
            {
                "JEP",
                "Jersey Pound (JEP)"
            },
            {
                "JOD",
                "Jordanian Dinar (JOD)"
            },
            {
                "KZT",
                "Kazakhstani Tenge (KZT)"
            },
            {
                "KES",
                "Kenyan Shilling (KES)"
            },
            {
                "KWD",
                "Kuwaiti Dinar (KWD)"
            },
            {
                "KGS",
                "Kyrgyzstani Som (KGS)"
            },
            {
                "LAK",
                "Lao Kip (LAK)"
            },
            {
                "LVL",
                "Latvian Lats (LVL)"
            },
            {
                "LBP",
                "Lebanese Pound (LBP)"
            },
            {
                "LSL",
                "Lesotho Loti (LSL)"
            },
            {
                "LRD",
                "Liberian Dollar (LRD)"
            },
            {
                "LYD",
                "Libyan Dinar (LYD)"
            },
            {
                "LTL",
                "Lithuanian Litas (LTL)"
            },
            {
                "MOP",
                "Macanese Pataca (MOP)"
            },
            {
                "MKD",
                "Macedonian Denar (MKD)"
            },
            {
                "MGA",
                "Malagasy Ariary (MGA)"
            },
            {
                "MWK",
                "Malawian Kwacha (MWK)"
            },
            {
                "MYR",
                "Malaysian Ringgit (MYR)"
            },
            {
                "MVR",
                "Maldivian Rufiyaa (MVR)"
            },
            {
                "MTL",
                "Maltese Lira (MTL)"
            },
            {
                "MRO",
                "Mauritanian Ouguiya (MRO)"
            },
            {
                "MUR",
                "Mauritian Rupee (MUR)"
            },
            {
                "MXN",
                "Mexican Peso (MXN)"
            },
            {
                "MDL",
                "Moldovan Leu (MDL)"
            },
            {
                "MNT",
                "Mongolian Tögrög (MNT)"
            },
            {
                "MAD",
                "Moroccan Dirham (MAD)"
            },
            {
                "MZN",
                "Mozambican Metical (MZN)"
            },
            {
                "MMK",
                "Myanmar Kyat (MMK)"
            },
            {
                "NAD",
                "Namibian Dollar (NAD)"
            },
            {
                "NPR",
                "Nepalese Rupee (NPR)"
            },
            {
                "ANG",
                "Netherlands Antillean Gulden (ANG)"
            },
            {
                "TWD",
                "New Taiwan Dollar (TWD)"
            },
            {
                "NZD",
                "New Zealand Dollar (NZD)"
            },
            {
                "NIO",
                "Nicaraguan Córdoba (NIO)"
            },
            {
                "NGN",
                "Nigerian Naira (NGN)"
            },
            {
                "NOK",
                "Norwegian Krone (NOK)"
            },
            {
                "OMR",
                "Omani Rial (OMR)"
            },
            {
                "PKR",
                "Pakistani Rupee (PKR)"
            },
            {
                "XPD",
                "Palladium (XPD)"
            },
            {
                "PAB",
                "Panamanian Balboa (PAB)"
            },
            {
                "PGK",
                "Papua New Guinean Kina (PGK)"
            },
            {
                "PYG",
                "Paraguayan Guaraní (PYG)"
            },
            {
                "PEN",
                "Peruvian Sol (PEN)"
            },
            {
                "PHP",
                "Philippine Peso (PHP)"
            },
            {
                "XPT",
                "Platinum (XPT)"
            },
            {
                "PLN",
                "Polish Z?oty (PLN)"
            },
            {
                "QAR",
                "Qatari Riyal (QAR)"
            },
            {
                "RON",
                "Romanian Leu (RON)"
            },
            {
                "RUB",
                "Russian Ruble (RUB)"
            },
            {
                "RWF",
                "Rwandan Franc (RWF)"
            },
            {
                "SHP",
                "Saint Helenian Pound (SHP)"
            },
            {
                "SVC",
                "Salvadoran Colón (SVC)"
            },
            {
                "WST",
                "Samoan Tala (WST)"
            },
            {
                "SAR",
                "Saudi Riyal (SAR)"
            },
            {
                "RSD",
                "Serbian Dinar (RSD)"
            },
            {
                "SCR",
                "Seychellois Rupee (SCR)"
            },
            {
                "SLL",
                "Sierra Leonean Leone (SLL)"
            },
            {
                "XAG",
                "Silver (Troy Ounce) (XAG)"
            },
            {
                "SGD",
                "Singapore Dollar (SGD)"
            },
            {
                "SBD",
                "Solomon Islands Dollar (SBD)"
            },
            {
                "SOS",
                "Somali Shilling (SOS)"
            },
            {
                "ZAR",
                "South African Rand (ZAR)"
            },
            {
                "KRW",
                "South Korean Won (KRW)"
            },
            {
                "SSP",
                "South Sudanese Pound (SSP)"
            },
            {
                "XDR",
                "Special Drawing Rights (XDR)"
            },
            {
                "LKR",
                "Sri Lankan Rupee (LKR)"
            },
            {
                "SRD",
                "Surinamese Dollar (SRD)"
            },
            {
                "SZL",
                "Swazi Lilangeni (SZL)"
            },
            {
                "SEK",
                "Swedish Krona (SEK)"
            },
            {
                "CHF",
                "Swiss Franc (CHF)"
            },
            {
                "STD",
                "S?o Tomé and Príncipe Dobra (STD)"
            },
            {
                "TJS",
                "Tajikistani Somoni (TJS)"
            },
            {
                "TZS",
                "Tanzanian Shilling (TZS)"
            },
            {
                "THB",
                "Thai Baht (THB)"
            },
            {
                "TOP",
                "Tongan Pa?anga (TOP)"
            },
            {
                "TTD",
                "Trinidad and Tobago Dollar (TTD)"
            },
            {
                "TND",
                "Tunisian Dinar (TND)"
            },
            {
                "TRY",
                "Turkish Lira (TRY)"
            },
            {
                "TMT",
                "Turkmenistani Manat (TMT)"
            },
            {
                "UGX",
                "Ugandan Shilling (UGX)"
            },
            {
                "UAH",
                "Ukrainian Hryvnia (UAH)"
            },
            {
                "CLF",
                "Unidad de Fomento (CLF)"
            },
            {
                "AED",
                "United Arab Emirates Dirham (AED)"
            },
            {
                "UYU",
                "Uruguayan Peso (UYU)"
            },
            {
                "UZS",
                "Uzbekistan Som (UZS)"
            },
            {
                "VUV",
                "Vanuatu Vatu (VUV)"
            },
            {
                "VEF",
                "Venezuelan Bolívar (VEF)"
            },
            {
                "VND",
                "Vietnamese Ðong (VND)"
            },
            {
                "XOF",
                "West African Cfa Franc (XOF)"
            },
            {
                "YER",
                "Yemeni Rial (YER)"
            },
            {
                "ZMK",
                "Zambian Kwacha (ZMK)"
            },
            {
                "ZMW",
                "Zambian Kwacha (ZMW)"
            },
            {
                "ZWL",
                "Zimbabwean Dollar (ZWL)"
            }
        };

        public static Dictionary<string, string> ListTimeZone1 = new Dictionary<string, string>
        {
            {"Africa/Abidjan", "+00:00"}, {"Africa/Accra", "+00:00"}, {"Africa/Addis_Ababa", "+03:00"},
            {"Africa/Algiers", "+01:00"}, {"Africa/Asmara", "+03:00"}, {"Africa/Bamako", "+00:00"},
            {"Africa/Bangui", "+01:00"}, {"Africa/Banjul", "+00:00"}, {"Africa/Bissau", "+00:00"},
            {"Africa/Blantyre", "+02:00"}, {"Africa/Brazzaville", "+01:00"}, {"Africa/Bujumbura", "+02:00"},
            {"Africa/Cairo", "+02:00"}, {"Africa/Casablanca", "+01:00"}, {"Africa/Ceuta", "+01:00"},
            {"Africa/Conakry", "+00:00"}, {"Africa/Dakar", "+00:00"}, {"Africa/Dar_es_Salaam", "+03:00"},
            {"Africa/Djibouti", "+03:00"}, {"Africa/Douala", "+01:00"}, {"Africa/El_Aaiun", "+00:00"},
            {"Africa/Freetown", "+00:00"}, {"Africa/Gaborone", "+02:00"}, {"Africa/Harare", "+02:00"},
            {"Africa/Johannesburg", "+02:00"}, {"Africa/Juba", "+03:00"}, {"Africa/Kampala", "+03:00"},
            {"Africa/Khartoum", "+02:00"}, {"Africa/Kigali", "+02:00"}, {"Africa/Kinshasa", "+01:00"},
            {"Africa/Lagos", "+01:00"}, {"Africa/Libreville", "+01:00"}, {"Africa/Lome", "+00:00"},
            {"Africa/Luanda", "+01:00"}, {"Africa/Lubumbashi", "+02:00"}, {"Africa/Lusaka", "+02:00"},
            {"Africa/Malabo", "+01:00"}, {"Africa/Maputo", "+02:00"}, {"Africa/Maseru", "+02:00"},
            {"Africa/Mbabane", "+02:00"}, {"Africa/Mogadishu", "+03:00"}, {"Africa/Monrovia", "+00:00"},
            {"Africa/Nairobi", "+03:00"}, {"Africa/Ndjamena", "+01:00"}, {"Africa/Niamey", "+01:00"},
            {"Africa/Nouakchott", "+00:00"}, {"Africa/Ouagadougou", "+00:00"}, {"Africa/Porto-Novo", "+01:00"},
            {"Africa/Sao_Tome", "+01:00"}, {"Africa/Timbuktu", "+00:00"}, {"Africa/Tripoli", "+02:00"},
            {"Africa/Tunis", "+01:00"}, {"Africa/Windhoek", "+02:00"}, {"America/Adak", "−10:00"},
            {"America/Anchorage", "−09:00"}, {"America/Anguilla", "−04:00"}, {"America/Antigua", "−04:00"},
            {"America/Araguaina", "−03:00"}, {"America/Argentina/Buenos_Aires", "−03:00"},
            {"America/Argentina/Catamarca", "−03:00"}, {"America/Argentina/ComodRivadavia", "−03:00"},
            {"America/Argentina/Cordoba", "−03:00"}, {"America/Argentina/Jujuy", "−03:00"},
            {"America/Argentina/La_Rioja", "−03:00"}, {"America/Argentina/Mendoza", "−03:00"},
            {"America/Argentina/Rio_Gallegos", "−03:00"}, {"America/Argentina/Salta", "−03:00"},
            {"America/Argentina/San_Juan", "−03:00"}, {"America/Argentina/San_Luis", "−03:00"},
            {"America/Argentina/Tucuman", "−03:00"}, {"America/Argentina/Ushuaia", "−03:00"},
            {"America/Aruba", "−04:00"}, {"America/Asuncion", "−04:00"}, {"America/Atikokan", "−05:00"},
            {"America/Atka", "−10:00"}, {"America/Bahia", "−03:00"}, {"America/Bahia_Banderas", "−06:00"},
            {"America/Barbados", "−04:00"}, {"America/Belem", "−03:00"}, {"America/Belize", "−06:00"},
            {"America/Blanc-Sablon", "−04:00"}, {"America/Boa_Vista", "−04:00"}, {"America/Bogota", "−05:00"},
            {"America/Boise", "−07:00"}, {"America/Buenos_Aires", "−03:00"}, {"America/Cambridge_Bay", "−07:00"},
            {"America/Campo_Grande", "−04:00"}, {"America/Cancun", "−05:00"}, {"America/Caracas", "−04:00"},
            {"America/Catamarca", "−03:00"}, {"America/Cayenne", "−03:00"}, {"America/Cayman", "−05:00"},
            {"America/Chicago", "−06:00"}, {"America/Chihuahua", "−07:00"}, {"America/Coral_Harbour", "−05:00"},
            {"America/Cordoba", "−03:00"}, {"America/Costa_Rica", "−06:00"}, {"America/Creston", "−07:00"},
            {"America/Cuiaba", "−04:00"}, {"America/Curacao", "−04:00"}, {"America/Danmarkshavn", "+00:00"},
            {"America/Dawson", "−08:00"}, {"America/Dawson_Creek", "−07:00"}, {"America/Denver", "−07:00"},
            {"America/Detroit", "−05:00"}, {"America/Dominica", "−04:00"}, {"America/Edmonton", "−07:00"},
            {"America/Eirunepe", "−05:00"}, {"America/El_Salvador", "−06:00"}, {"America/Ensenada", "−08:00"},
            {"America/Fort_Nelson", "−07:00"}, {"America/Fort_Wayne", "−05:00"}, {"America/Fortaleza", "−03:00"},
            {"America/Glace_Bay", "−04:00"}, {"America/Godthab", "−03:00"}, {"America/Goose_Bay", "−04:00"},
            {"America/Grand_Turk", "−04:00"}, {"America/Grenada", "−04:00"}, {"America/Guadeloupe", "−04:00"},
            {"America/Guatemala", "−06:00"}, {"America/Guayaquil", "−05:00"}, {"America/Guyana", "−04:00"},
            {"America/Halifax", "−04:00"}, {"America/Havana", "−05:00"}, {"America/Hermosillo", "−07:00"},
            {"America/Indiana/Indianapolis", "−05:00"}, {"America/Indiana/Knox", "−06:00"},
            {"America/Indiana/Marengo", "−05:00"}, {"America/Indiana/Petersburg", "−05:00"},
            {"America/Indiana/Tell_City", "−06:00"}, {"America/Indiana/Vevay", "−05:00"},
            {"America/Indiana/Vincennes", "−05:00"}, {"America/Indiana/Winamac", "−05:00"},
            {"America/Indianapolis", "−05:00"}, {"America/Inuvik", "−07:00"}, {"America/Iqaluit", "−05:00"},
            {"America/Jamaica", "−05:00"}, {"America/Jujuy", "−03:00"}, {"America/Juneau", "−09:00"},
            {"America/Kentucky/Louisville", "−05:00"}, {"America/Kentucky/Monticello", "−05:00"},
            {"America/Knox_IN", "−06:00"}, {"America/Kralendijk", "−04:00"}, {"America/La_Paz", "−04:00"},
            {"America/Lima", "−05:00"}, {"America/Los_Angeles", "−08:00"}, {"America/Louisville", "−05:00"},
            {"America/Lower_Princes", "−04:00"}, {"America/Maceio", "−03:00"}, {"America/Managua", "−06:00"},
            {"America/Manaus", "−04:00"}, {"America/Marigot", "−04:00"}, {"America/Martinique", "−04:00"},
            {"America/Matamoros", "−06:00"}, {"America/Mazatlan", "−07:00"}, {"America/Mendoza", "−03:00"},
            {"America/Menominee", "−06:00"}, {"America/Merida", "−06:00"}, {"America/Metlakatla", "−09:00"},
            {"America/Mexico_City", "−06:00"}, {"America/Miquelon", "−03:00"}, {"America/Moncton", "−04:00"},
            {"America/Monterrey", "−06:00"}, {"America/Montevideo", "−03:00"}, {"America/Montreal", "−05:00"},
            {"America/Montserrat", "−04:00"}, {"America/Nassau", "−05:00"}, {"America/New_York", "−05:00"},
            {"America/Nipigon", "−05:00"}, {"America/Nome", "−09:00"}, {"America/Noronha", "−02:00"},
            {"America/North_Dakota/Beulah", "−06:00"}, {"America/North_Dakota/Center", "−06:00"},
            {"America/North_Dakota/New_Salem", "−06:00"}, {"America/Ojinaga", "−07:00"}, {"America/Panama", "−05:00"},
            {"America/Pangnirtung", "−05:00"}, {"America/Paramaribo", "−03:00"}, {"America/Phoenix", "−07:00"},
            {"America/Port_of_Spain", "−04:00"}, {"America/Port-au-Prince", "−05:00"}, {"America/Porto_Acre", "−05:00"},
            {"America/Porto_Velho", "−04:00"}, {"America/Puerto_Rico", "−04:00"}, {"America/Punta_Arenas", "−03:00"},
            {"America/Rainy_River", "−06:00"}, {"America/Rankin_Inlet", "−06:00"}, {"America/Recife", "−03:00"},
            {"America/Regina", "−06:00"}, {"America/Resolute", "−06:00"}, {"America/Rio_Branco", "−05:00"},
            {"America/Rosario", "−03:00"}, {"America/Santa_Isabel", "−08:00"}, {"America/Santarem", "−03:00"},
            {"America/Santiago", "−04:00"}, {"America/Santo_Domingo", "−04:00"}, {"America/Sao_Paulo", "−03:00"},
            {"America/Scoresbysund", "−01:00"}, {"America/Shiprock", "−07:00"}, {"America/Sitka", "−09:00"},
            {"America/St_Barthelemy", "−04:00"}, {"America/St_Johns", "−03:30"}, {"America/St_Kitts", "−04:00"},
            {"America/St_Lucia", "−04:00"}, {"America/St_Thomas", "−04:00"}, {"America/St_Vincent", "−04:00"},
            {"America/Swift_Current", "−06:00"}, {"America/Tegucigalpa", "−06:00"}, {"America/Thule", "−04:00"},
            {"America/Thunder_Bay", "−05:00"}, {"America/Tijuana", "−08:00"}, {"America/Toronto", "−05:00"},
            {"America/Tortola", "−04:00"}, {"America/Vancouver", "−08:00"}, {"America/Virgin", "−04:00"},
            {"America/Whitehorse", "−08:00"}, {"America/Winnipeg", "−06:00"}, {"America/Yakutat", "−09:00"},
            {"America/Yellowknife", "−07:00"}, {"Antarctica/Casey", "+11:00"}, {"Antarctica/Davis", "+07:00"},
            {"Antarctica/DumontDUrville", "+10:00"}, {"Antarctica/Macquarie", "+11:00"},
            {"Antarctica/Mawson", "+05:00"}, {"Antarctica/McMurdo", "+12:00"}, {"Antarctica/Palmer", "−03:00"},
            {"Antarctica/Rothera", "−03:00"}, {"Antarctica/South_Pole", "+12:00"}, {"Antarctica/Syowa", "+03:00"},
            {"Antarctica/Troll", "+00:00"}, {"Antarctica/Vostok", "+06:00"}, {"Arctic/Longyearbyen", "+01:00"},
            {"Asia/Aden", "+03:00"}, {"Asia/Almaty", "+06:00"}, {"Asia/Amman", "+02:00"}, {"Asia/Anadyr", "+12:00"},
            {"Asia/Aqtau", "+05:00"}, {"Asia/Aqtobe", "+05:00"}, {"Asia/Ashgabat", "+05:00"},
            {"Asia/Ashkhabad", "+05:00"}, {"Asia/Atyrau", "+05:00"}, {"Asia/Baghdad", "+03:00"},
            {"Asia/Bahrain", "+03:00"}, {"Asia/Baku", "+04:00"}, {"Asia/Bangkok", "+07:00"}, {"Asia/Barnaul", "+07:00"},
            {"Asia/Beirut", "+02:00"}, {"Asia/Bishkek", "+06:00"}, {"Asia/Brunei", "+08:00"},
            {"Asia/Calcutta", "+05:30"}, {"Asia/Chita", "+09:00"}, {"Asia/Choibalsan", "+08:00"},
            {"Asia/Chongqing", "+08:00"}, {"Asia/Chungking", "+08:00"}, {"Asia/Colombo", "+05:30"},
            {"Asia/Dacca", "+06:00"}, {"Asia/Damascus", "+02:00"}, {"Asia/Dhaka", "+06:00"}, {"Asia/Dili", "+09:00"},
            {"Asia/Dubai", "+04:00"}, {"Asia/Dushanbe", "+05:00"}, {"Asia/Famagusta", "+02:00"},
            {"Asia/Gaza", "+02:00"}, {"Asia/Harbin", "+08:00"}, {"Asia/Hebron", "+02:00"},
            {"Asia/Ho_Chi_Minh", "+07:00"}, {"Asia/Hong_Kong", "+08:00"}, {"Asia/Hovd", "+07:00"},
            {"Asia/Irkutsk", "+08:00"}, {"Asia/Istanbul", "+03:00"}, {"Asia/Jakarta", "+07:00"},
            {"Asia/Jayapura", "+09:00"}, {"Asia/Jerusalem", "+02:00"}, {"Asia/Kabul", "+04:30"},
            {"Asia/Kamchatka", "+12:00"}, {"Asia/Karachi", "+05:00"}, {"Asia/Kashgar", "+06:00"},
            {"Asia/Kathmandu", "+05:45"}, {"Asia/Katmandu", "+05:45"}, {"Asia/Khandyga", "+09:00"},
            {"Asia/Kolkata", "+05:30"}, {"Asia/Krasnoyarsk", "+07:00"}, {"Asia/Kuala_Lumpur", "+08:00"},
            {"Asia/Kuching", "+08:00"}, {"Asia/Kuwait", "+03:00"}, {"Asia/Macao", "+08:00"}, {"Asia/Macau", "+08:00"},
            {"Asia/Magadan", "+11:00"}, {"Asia/Makassar", "+08:00"}, {"Asia/Manila", "+08:00"},
            {"Asia/Muscat", "+04:00"}, {"Asia/Novokuznetsk", "+07:00"}, {"Asia/Novosibirsk", "+07:00"},
            {"Asia/Omsk", "+06:00"}, {"Asia/Oral", "+05:00"}, {"Asia/Phnom_Penh", "+07:00"},
            {"Asia/Pontianak", "+07:00"}, {"Asia/Pyongyang", "+09:00"}, {"Asia/Qatar", "+03:00"},
            {"Asia/Qyzylorda", "+06:00"}, {"Asia/Rangoon", "+06:30"}, {"Asia/Riyadh", "+03:00"},
            {"Asia/Saigon", "+07:00"}, {"Asia/Sakhalin", "+11:00"}, {"Asia/Samarkand", "+05:00"},
            {"Asia/Seoul", "+09:00"}, {"Asia/Shanghai", "+08:00"}, {"Asia/Singapore", "+08:00"},
            {"Asia/Srednekolymsk", "+11:00"}, {"Asia/Taipei", "+08:00"}, {"Asia/Tashkent", "+05:00"},
            {"Asia/Tbilisi", "+04:00"}, {"Asia/Tehran", "+03:30"}, {"Asia/Tel_Aviv", "+02:00"},
            {"Asia/Thimbu", "+06:00"}, {"Asia/Thimphu", "+06:00"}, {"Asia/Tokyo", "+09:00"}, {"Asia/Tomsk", "+07:00"},
            {"Asia/Ujung_Pandang", "+08:00"}, {"Asia/Ulaanbaatar", "+08:00"}, {"Asia/Ulan_Bator", "+08:00"},
            {"Asia/Urumqi", "+06:00"}, {"Asia/Ust-Nera", "+10:00"}, {"Asia/Vientiane", "+07:00"},
            {"Asia/Vladivostok", "+10:00"}, {"Asia/Yakutsk", "+09:00"}, {"Asia/Yangon", "+06:30"},
            {"Asia/Yekaterinburg", "+05:00"}, {"Asia/Yerevan", "+04:00"}, {"Atlantic/Azores", "−01:00"},
            {"Atlantic/Bermuda", "−04:00"}, {"Atlantic/Canary", "+00:00"}, {"Atlantic/Cape_Verde", "−01:00"},
            {"Atlantic/Faeroe", "+00:00"}, {"Atlantic/Faroe", "+00:00"}, {"Atlantic/Jan_Mayen", "+01:00"},
            {"Atlantic/Madeira", "+00:00"}, {"Atlantic/Reykjavik", "+00:00"}, {"Atlantic/South_Georgia", "−02:00"},
            {"Atlantic/St_Helena", "+00:00"}, {"Atlantic/Stanley", "−03:00"}, {"Australia/ACT", "+10:00"},
            {"Australia/Adelaide", "+09:30"}, {"Australia/Brisbane", "+10:00"}, {"Australia/Broken_Hill", "+09:30"},
            {"Australia/Canberra", "+10:00"}, {"Australia/Currie", "+10:00"}, {"Australia/Darwin", "+09:30"},
            {"Australia/Eucla", "+08:45"}, {"Australia/Hobart", "+10:00"}, {"Australia/LHI", "+10:30"},
            {"Australia/Lindeman", "+10:00"}, {"Australia/Lord_Howe", "+10:30"}, {"Australia/Melbourne", "+10:00"},
            {"Australia/North", "+09:30"}, {"Australia/NSW", "+10:00"}, {"Australia/Perth", "+08:00"},
            {"Australia/Queensland", "+10:00"}, {"Australia/South", "+09:30"}, {"Australia/Sydney", "+10:00"},
            {"Australia/Tasmania", "+10:00"}, {"Australia/Victoria", "+10:00"}, {"Australia/West", "+08:00"},
            {"Australia/Yancowinna", "+09:30"}, {"Brazil/Acre", "−05:00"}, {"Brazil/DeNoronha", "−02:00"},
            {"Brazil/East", "−03:00"}, {"Brazil/West", "−04:00"}, {"Canada/Atlantic", "−04:00"},
            {"Canada/Central", "−06:00"}, {"Canada/Eastern", "−05:00"}, {"Canada/Mountain", "−07:00"},
            {"Canada/Newfoundland", "−03:30"}, {"Canada/Pacific", "−08:00"}, {"Canada/Saskatchewan", "−06:00"},
            {"Canada/Yukon", "−08:00"}, {"CET", "+01:00"}, {"Chile/Continental", "−04:00"},
            {"Chile/EasterIsland", "−06:00"}, {"CST6CDT", "−06:00"}, {"Cuba", "−05:00"}, {"EET", "+02:00"},
            {"Egypt", "+02:00"}, {"Eire", "+00:00"}, {"EST", "−05:00"}, {"EST5EDT", "−05:00"}, {"Etc/GMT", "+00:00"},
            {"Etc/GMT+0", "+00:00"}, {"Etc/GMT+1", "−01:00"}, {"Etc/GMT+10", "−10:00"}, {"Etc/GMT+11", "−11:00"},
            {"Etc/GMT+12", "−12:00"}, {"Etc/GMT+2", "−02:00"}, {"Etc/GMT+3", "−03:00"}, {"Etc/GMT+4", "−04:00"},
            {"Etc/GMT+5", "−05:00"}, {"Etc/GMT+6", "−06:00"}, {"Etc/GMT+7", "−07:00"}, {"Etc/GMT+8", "−08:00"},
            {"Etc/GMT+9", "−09:00"}, {"Etc/GMT0", "+00:00"}, {"Etc/GMT-0", "+00:00"}, {"Etc/GMT-1", "+01:00"},
            {"Etc/GMT-10", "+10:00"}, {"Etc/GMT-11", "+11:00"}, {"Etc/GMT-12", "+12:00"}, {"Etc/GMT-13", "+13:00"},
            {"Etc/GMT-14", "+14:00"}, {"Etc/GMT-2", "+02:00"}, {"Etc/GMT-3", "+03:00"}, {"Etc/GMT-4", "+04:00"},
            {"Etc/GMT-5", "+05:00"}, {"Etc/GMT-6", "+06:00"}, {"Etc/GMT-7", "+07:00"}, {"Etc/GMT-8", "+08:00"},
            {"Etc/GMT-9", "+09:00"}, {"Etc/Greenwich", "+00:00"}, {"Etc/UCT", "+00:00"}, {"Etc/Universal", "+00:00"},
            {"Etc/UTC", "+00:00"}, {"Etc/Zulu", "+00:00"}, {"Europe/Amsterdam", "+01:00"}, {"Europe/Andorra", "+01:00"},
            {"Europe/Astrakhan", "+04:00"}, {"Europe/Athens", "+02:00"}, {"Europe/Belfast", "+00:00"},
            {"Europe/Belgrade", "+01:00"}, {"Europe/Berlin", "+01:00"}, {"Europe/Bratislava", "+01:00"},
            {"Europe/Brussels", "+01:00"}, {"Europe/Bucharest", "+02:00"}, {"Europe/Budapest", "+01:00"},
            {"Europe/Busingen", "+01:00"}, {"Europe/Chisinau", "+02:00"}, {"Europe/Copenhagen", "+01:00"},
            {"Europe/Dublin", "+00:00"}, {"Europe/Gibraltar", "+01:00"}, {"Europe/Guernsey", "+00:00"},
            {"Europe/Helsinki", "+02:00"}, {"Europe/Isle_of_Man", "+00:00"}, {"Europe/Istanbul", "+03:00"},
            {"Europe/Jersey", "+00:00"}, {"Europe/Kaliningrad", "+02:00"}, {"Europe/Kyiv", "+02:00"},
            {"Europe/Kirov", "+03:00"}, {"Europe/Lisbon", "+00:00"}, {"Europe/Ljubljana", "+01:00"},
            {"Europe/London", "+00:00"}, {"Europe/Luxembourg", "+01:00"}, {"Europe/Madrid", "+01:00"},
            {"Europe/Malta", "+01:00"}, {"Europe/Mariehamn", "+02:00"}, {"Europe/Minsk", "+03:00"},
            {"Europe/Monaco", "+01:00"}, {"Europe/Moscow", "+03:00"}, {"Europe/Nicosia", "+02:00"},
            {"Europe/Oslo", "+01:00"}, {"Europe/Paris", "+01:00"}, {"Europe/Podgorica", "+01:00"},
            {"Europe/Prague", "+01:00"}, {"Europe/Riga", "+02:00"}, {"Europe/Rome", "+01:00"},
            {"Europe/Samara", "+04:00"}, {"Europe/San_Marino", "+01:00"}, {"Europe/Sarajevo", "+01:00"},
            {"Europe/Saratov", "+04:00"}, {"Europe/Simferopol", "+03:00"}, {"Europe/Skopje", "+01:00"},
            {"Europe/Sofia", "+02:00"}, {"Europe/Stockholm", "+01:00"}, {"Europe/Tallinn", "+02:00"},
            {"Europe/Tirane", "+01:00"}, {"Europe/Tiraspol", "+02:00"}, {"Europe/Ulyanovsk", "+04:00"},
            {"Europe/Uzhgorod", "+02:00"}, {"Europe/Vaduz", "+01:00"}, {"Europe/Vatican", "+01:00"},
            {"Europe/Vienna", "+01:00"}, {"Europe/Vilnius", "+02:00"}, {"Europe/Volgograd", "+04:00"},
            {"Europe/Warsaw", "+01:00"}, {"Europe/Zagreb", "+01:00"}, {"Europe/Zaporozhye", "+02:00"},
            {"Europe/Zurich", "+01:00"}, {"GB", "+00:00"}, {"GB-Eire", "+00:00"}, {"GMT", "+00:00"},
            {"GMT+0", "+00:00"}, {"GMT0", "+00:00"}, {"GMT−0", "+00:00"}, {"Greenwich", "+00:00"},
            {"Hongkong", "+08:00"}, {"HST", "−10:00"}, {"Iceland", "+00:00"}, {"Indian/Antananarivo", "+03:00"},
            {"Indian/Chagos", "+06:00"}, {"Indian/Christmas", "+07:00"}, {"Indian/Cocos", "+06:30"},
            {"Indian/Comoro", "+03:00"}, {"Indian/Kerguelen", "+05:00"}, {"Indian/Mahe", "+04:00"},
            {"Indian/Maldives", "+05:00"}, {"Indian/Mauritius", "+04:00"}, {"Indian/Mayotte", "+03:00"},
            {"Indian/Reunion", "+04:00"}, {"Iran", "+03:30"}, {"Israel", "+02:00"}, {"Jamaica", "−05:00"},
            {"Japan", "+09:00"}, {"Kwajalein", "+12:00"}, {"Libya", "+02:00"}, {"MET", "+01:00"},
            {"Mexico/BajaNorte", "−08:00"}, {"Mexico/BajaSur", "−07:00"}, {"Mexico/General", "−06:00"},
            {"MST", "−07:00"}, {"MST7MDT", "−07:00"}, {"Navajo", "−07:00"}, {"NZ", "+12:00"}, {"NZ-CHAT", "+12:45"},
            {"Pacific/Apia", "+13:00"}, {"Pacific/Auckland", "+12:00"}, {"Pacific/Bougainville", "+11:00"},
            {"Pacific/Chatham", "+12:45"}, {"Pacific/Chuuk", "+10:00"}, {"Pacific/Easter", "−06:00"},
            {"Pacific/Efate", "+11:00"}, {"Pacific/Enderbury", "+13:00"}, {"Pacific/Fakaofo", "+13:00"},
            {"Pacific/Fiji", "+12:00"}, {"Pacific/Funafuti", "+12:00"}, {"Pacific/Galapagos", "−06:00"},
            {"Pacific/Gambier", "−09:00"}, {"Pacific/Guadalcanal", "+11:00"}, {"Pacific/Guam", "+10:00"},
            {"Pacific/Honolulu", "−10:00"}, {"Pacific/Johnston", "−10:00"}, {"Pacific/Kiritimati", "+14:00"},
            {"Pacific/Kosrae", "+11:00"}, {"Pacific/Kwajalein", "+12:00"}, {"Pacific/Majuro", "+12:00"},
            {"Pacific/Marquesas", "−09:30"}, {"Pacific/Midway", "−11:00"}, {"Pacific/Nauru", "+12:00"},
            {"Pacific/Niue", "−11:00"}, {"Pacific/Norfolk", "+11:00"}, {"Pacific/Noumea", "+11:00"},
            {"Pacific/Pago_Pago", "−11:00"}, {"Pacific/Palau", "+09:00"}, {"Pacific/Pitcairn", "−08:00"},
            {"Pacific/Pohnpei", "+11:00"}, {"Pacific/Ponape", "+11:00"}, {"Pacific/Port_Moresby", "+10:00"},
            {"Pacific/Rarotonga", "−10:00"}, {"Pacific/Saipan", "+10:00"}, {"Pacific/Samoa", "−11:00"},
            {"Pacific/Tahiti", "−10:00"}, {"Pacific/Tarawa", "+12:00"}, {"Pacific/Tongatapu", "+13:00"},
            {"Pacific/Truk", "+10:00"}, {"Pacific/Wake", "+12:00"}, {"Pacific/Wallis", "+12:00"},
            {"Pacific/Yap", "+10:00"}, {"Poland", "+01:00"}, {"Portugal", "+00:00"}, {"PRC", "+08:00"},
            {"PST8PDT", "−08:00"}, {"ROC", "+08:00"}, {"ROK", "+09:00"}, {"Singapore", "+08:00"}, {"Turkey", "+03:00"},
            {"UCT", "+00:00"}, {"Universal", "+00:00"}, {"US/Alaska", "−09:00"}, {"US/Aleutian", "−10:00"},
            {"US/Arizona", "−07:00"}, {"US/Central", "−06:00"}, {"US/Eastern", "−05:00"}, {"US/East-Indiana", "−05:00"},
            {"US/Hawaii", "−10:00"}, {"US/Indiana-Starke", "−06:00"}, {"US/Michigan", "−05:00"},
            {"US/Mountain", "−07:00"}, {"US/Pacific", "−08:00"}, {"US/Pacific-New", "−08:00"}, {"US/Samoa", "−11:00"},
            {"UTC", "+00:00"}, {"WET", "+00:00"}, {"W-SU", "+03:00"}, {"Zulu", "+00:00"}
        };
    }
}