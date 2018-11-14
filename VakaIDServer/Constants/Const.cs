using System.Collections.Generic;
using VakaxaIDServer.Models;

namespace VakaxaIDServer.Constants
{
    public static class Const
    {
        //Purpose create token
        public const string TypeGenerateChangePassword = "ChangePassword";
        public const string TypeGenerateChangeOldPhoneNumber = "ChangePhoneOldPhone";
        public const string TypeGenerateChangeNewPhoneNumber = "ChangePhoneNewPhone";
        public const string TypeGenerateVerifyPhoneNumberAgain = "VerifyPhoneAgain";
        public const string TypeGenerateChangeTwoFactor = "ChangeTwoFactor";
        public const string TypeGenerateLockAccount = "LockAccount";
        public const string TypeGenerateUnlockAccount = "UnlockAccount";
        public const string TypeGenerateAddPhoneNumber = "AddPhoneNumber";
        public const string TypeGenerateAddLockScreen = "AddLockScreen";
        public const string TypeGenerateLogin = "Login";

        //Provider token
        public const string ProviderEmail = "Email";
        public const string ProviderPhone = "Phone";
        public const string ProviderDefault = "Default";

        //Type Verify email
        public const int TypeVerifyEmailRegister = 1;
        public const int TypeVerifyEmailUnlockAccount = 2;

        //Type Send SMS
        public const int TypeSendSmsRegister = 1;
        public const int TypeSendSmsTwoFactor = 2;
        public const int TypeSendSmsUnLock = 3;
        
        //CookieSessionId
        public const string CookieSessionId = "__ssid__";

        public static readonly List<CountryModel> ListCountryModels = new List<CountryModel>
        {
            new CountryModel
            {
                Name = "Afghanistan",
                CallingCode = "+93",
                Code = "AF"
            },
            new CountryModel
            {
                Name = "Aland Islands",
                CallingCode = "+358",
                Code = "AX"
            },
            new CountryModel
            {
                Name = "Albania",
                CallingCode = "+355",
                Code = "AL"
            },
            new CountryModel
            {
                Name = "Algeria",
                CallingCode = "+213",
                Code = "DZ"
            },
            new CountryModel
            {
                Name = "AmericanSamoa",
                CallingCode = "+1684",
                Code = "AS"
            },
            new CountryModel
            {
                Name = "Andorra",
                CallingCode = "+376",
                Code = "AD"
            },
            new CountryModel
            {
                Name = "Angola",
                CallingCode = "+244",
                Code = "AO"
            },
            new CountryModel
            {
                Name = "Anguilla",
                CallingCode = "+1264",
                Code = "AI"
            },
            new CountryModel
            {
                Name = "Antarctica",
                CallingCode = "+672",
                Code = "AQ"
            },
            new CountryModel
            {
                Name = "Antigua and Barbuda",
                CallingCode = "+1268",
                Code = "AG"
            },
            new CountryModel
            {
                Name = "Argentina",
                CallingCode = "+54",
                Code = "AR"
            },
            new CountryModel
            {
                Name = "Armenia",
                CallingCode = "+374",
                Code = "AM"
            },
            new CountryModel
            {
                Name = "Aruba",
                CallingCode = "+297",
                Code = "AW"
            },
            new CountryModel
            {
                Name = "Australia",
                CallingCode = "+61",
                Code = "AU"
            },
            new CountryModel
            {
                Name = "Austria",
                CallingCode = "+43",
                Code = "AT"
            },
            new CountryModel
            {
                Name = "Azerbaijan",
                CallingCode = "+994",
                Code = "AZ"
            },
            new CountryModel
            {
                Name = "Bahamas",
                CallingCode = "+1242",
                Code = "BS"
            },
            new CountryModel
            {
                Name = "Bahrain",
                CallingCode = "+973",
                Code = "BH"
            },
            new CountryModel
            {
                Name = "Bangladesh",
                CallingCode = "+880",
                Code = "BD"
            },
            new CountryModel
            {
                Name = "Barbados",
                CallingCode = "+1246",
                Code = "BB"
            },
            new CountryModel
            {
                Name = "Belarus",
                CallingCode = "+375",
                Code = "BY"
            },
            new CountryModel
            {
                Name = "Belgium",
                CallingCode = "+32",
                Code = "BE"
            },
            new CountryModel
            {
                Name = "Belize",
                CallingCode = "+501",
                Code = "BZ"
            },
            new CountryModel
            {
                Name = "Benin",
                CallingCode = "+229",
                Code = "BJ"
            },
            new CountryModel
            {
                Name = "Bermuda",
                CallingCode = "+1441",
                Code = "BM"
            },
            new CountryModel
            {
                Name = "Bhutan",
                CallingCode = "+975",
                Code = "BT"
            },
            new CountryModel
            {
                Name = "Bolivia, Plurinational State of Bolivia",
                CallingCode = "+591",
                Code = "BO"
            },
            new CountryModel
            {
                Name = "Bonaire, Sint Eustatius and Saba",
                CallingCode = "+599",
                Code = "BQ"
            },
            new CountryModel
            {
                Name = "Bosnia and Herzegovina",
                CallingCode = "+387",
                Code = "BA"
            },
            new CountryModel
            {
                Name = "Botswana",
                CallingCode = "+267",
                Code = "BW"
            },
            new CountryModel
            {
                Name = "Bouvet Island",
                CallingCode = "+55",
                Code = "BV"
            },
            new CountryModel
            {
                Name = "Brazil",
                CallingCode = "+55",
                Code = "BR"
            },
            new CountryModel
            {
                Name = "British Indian Ocean Territory",
                CallingCode = "+246",
                Code = "IO"
            },
            new CountryModel
            {
                Name = "Brunei Darussalam",
                CallingCode = "+673",
                Code = "BN"
            },
            new CountryModel
            {
                Name = "Bulgaria",
                CallingCode = "+359",
                Code = "BG"
            },
            new CountryModel
            {
                Name = "Burkina Faso",
                CallingCode = "+226",
                Code = "BF"
            },
            new CountryModel
            {
                Name = "Burundi",
                CallingCode = "+257",
                Code = "BI"
            },
            new CountryModel
            {
                Name = "Cambodia",
                CallingCode = "+855",
                Code = "KH"
            },
            new CountryModel
            {
                Name = "Cameroon",
                CallingCode = "+237",
                Code = "CM"
            },
            new CountryModel
            {
                Name = "Canada",
                CallingCode = "+1",
                Code = "CA"
            },
            new CountryModel
            {
                Name = "Cape Verde",
                CallingCode = "+238",
                Code = "CV"
            },
            new CountryModel
            {
                Name = "Cayman Islands",
                CallingCode = "+1345",
                Code = "KY"
            },
            new CountryModel
            {
                Name = "Central African Republic",
                CallingCode = "+236",
                Code = "CF"
            },
            new CountryModel
            {
                Name = "Chad",
                CallingCode = "+235",
                Code = "TD"
            },
            new CountryModel
            {
                Name = "Chile",
                CallingCode = "+56",
                Code = "CL"
            },
            new CountryModel
            {
                Name = "China",
                CallingCode = "+86",
                Code = "CN"
            },
            new CountryModel
            {
                Name = "Christmas Island",
                CallingCode = "+61",
                Code = "CX"
            },
            new CountryModel
            {
                Name = "Cocos (Keeling) Islands",
                CallingCode = "+61",
                Code = "CC"
            },
            new CountryModel
            {
                Name = "Colombia",
                CallingCode = "+57",
                Code = "CO"
            },
            new CountryModel
            {
                Name = "Comoros",
                CallingCode = "+269",
                Code = "KM"
            },
            new CountryModel
            {
                Name = "Congo",
                CallingCode = "+242",
                Code = "CG"
            },
            new CountryModel
            {
                Name = "Congo, The Democratic Republic of Congo",
                CallingCode = "+243",
                Code = "CD"
            },
            new CountryModel
            {
                Name = "Cook Islands",
                CallingCode = "+682",
                Code = "CK"
            },
            new CountryModel
            {
                Name = "Costa Rica",
                CallingCode = "+506",
                Code = "CR"
            },
            new CountryModel
            {
                Name = "Cote d'Ivoire",
                CallingCode = "+225",
                Code = "CI"
            },
            new CountryModel
            {
                Name = "Croatia",
                CallingCode = "+385",
                Code = "HR"
            },
            new CountryModel
            {
                Name = "Cuba",
                CallingCode = "+53",
                Code = "CU"
            },
            new CountryModel
            {
                Name = "Curaçao",
                CallingCode = "+5999",
                Code = "CW"
            },
            new CountryModel
            {
                Name = "Cyprus",
                CallingCode = "+357",
                Code = "CY"
            },
            new CountryModel
            {
                Name = "Czech Republic",
                CallingCode = "+420",
                Code = "CZ"
            },
            new CountryModel
            {
                Name = "Denmark",
                CallingCode = "+45",
                Code = "DK"
            },
            new CountryModel
            {
                Name = "Djibouti",
                CallingCode = "+253",
                Code = "DJ"
            },
            new CountryModel
            {
                Name = "Dominica",
                CallingCode = "+1767",
                Code = "DM"
            },
            new CountryModel
            {
                Name = "Dominican Republic",
                CallingCode = "+1849",
                Code = "DO"
            },
            new CountryModel
            {
                Name = "Ecuador",
                CallingCode = "+593",
                Code = "EC"
            },
            new CountryModel
            {
                Name = "Egypt",
                CallingCode = "+20",
                Code = "EG"
            },
            new CountryModel
            {
                Name = "El Salvador",
                CallingCode = "+503",
                Code = "SV"
            },
            new CountryModel
            {
                Name = "Equatorial Guinea",
                CallingCode = "+240",
                Code = "GQ"
            },
            new CountryModel
            {
                Name = "Eritrea",
                CallingCode = "+291",
                Code = "ER"
            },
            new CountryModel
            {
                Name = "Estonia",
                CallingCode = "+372",
                Code = "EE"
            },
            new CountryModel
            {
                Name = "Ethiopia",
                CallingCode = "+251",
                Code = "ET"
            },
            new CountryModel
            {
                Name = "Falkland Islands (Malvinas)",
                CallingCode = "+500",
                Code = "FK"
            },
            new CountryModel
            {
                Name = "Faroe Islands",
                CallingCode = "+298",
                Code = "FO"
            },
            new CountryModel
            {
                Name = "Fiji",
                CallingCode = "+679",
                Code = "FJ"
            },
            new CountryModel
            {
                Name = "Finland",
                CallingCode = "+358",
                Code = "FI"
            },
            new CountryModel
            {
                Name = "France",
                CallingCode = "+33",
                Code = "FR"
            },
            new CountryModel
            {
                Name = "French Guiana",
                CallingCode = "+594",
                Code = "GF"
            },
            new CountryModel
            {
                Name = "French Polynesia",
                CallingCode = "+689",
                Code = "PF"
            },
            new CountryModel
            {
                Name = "French Southern and Antarctic Lands",
                CallingCode = "+262",
                Code = "TF"
            },
            new CountryModel
            {
                Name = "Gabon",
                CallingCode = "+241",
                Code = "GA"
            },
            new CountryModel
            {
                Name = "Gambia",
                CallingCode = "+220",
                Code = "GM"
            },
            new CountryModel
            {
                Name = "Georgia",
                CallingCode = "+995",
                Code = "GE"
            },
            new CountryModel
            {
                Name = "Germany",
                CallingCode = "+49",
                Code = "DE"
            },
            new CountryModel
            {
                Name = "Ghana",
                CallingCode = "+233",
                Code = "GH"
            },
            new CountryModel
            {
                Name = "Gibraltar",
                CallingCode = "+350",
                Code = "GI"
            },
            new CountryModel
            {
                Name = "Greece",
                CallingCode = "+30",
                Code = "GR"
            },
            new CountryModel
            {
                Name = "Greenland",
                CallingCode = "+299",
                Code = "GL"
            },
            new CountryModel
            {
                Name = "Grenada",
                CallingCode = "+1473",
                Code = "GD"
            },
            new CountryModel
            {
                Name = "Guadeloupe",
                CallingCode = "+590",
                Code = "GP"
            },
            new CountryModel
            {
                Name = "Guam",
                CallingCode = "+1671",
                Code = "GU"
            },
            new CountryModel
            {
                Name = "Guatemala",
                CallingCode = "+502",
                Code = "GT"
            },
            new CountryModel
            {
                Name = "Guernsey",
                CallingCode = "+44",
                Code = "GG"
            },
            new CountryModel
            {
                Name = "Guinea",
                CallingCode = "+224",
                Code = "GN"
            },
            new CountryModel
            {
                Name = "Guinea-Bissau",
                CallingCode = "+245",
                Code = "GW"
            },
            new CountryModel
            {
                Name = "Guyana",
                CallingCode = "+592",
                Code = "GY"
            },
            new CountryModel
            {
                Name = "Haiti",
                CallingCode = "+509",
                Code = "HT"
            },
            new CountryModel
            {
                Name = "Heard Island and McDonald Islands",
                CallingCode = "+672",
                Code = "HM"
            },
            new CountryModel
            {
                Name = "Holy See (Vatican City State)",
                CallingCode = "+379",
                Code = "VA"
            },
            new CountryModel
            {
                Name = "Honduras",
                CallingCode = "+504",
                Code = "HN"
            },
            new CountryModel
            {
                Name = "Hong Kong",
                CallingCode = "+852",
                Code = "HK"
            },
            new CountryModel
            {
                Name = "Hungary",
                CallingCode = "+36",
                Code = "HU"
            },
            new CountryModel
            {
                Name = "Iceland",
                CallingCode = "+354",
                Code = "IS"
            },
            new CountryModel
            {
                Name = "India",
                CallingCode = "+91",
                Code = "IN"
            },
            new CountryModel
            {
                Name = "Indonesia",
                CallingCode = "+62",
                Code = "ID"
            },
            new CountryModel
            {
                Name = "Iran, Islamic Republic of Persian Gulf",
                CallingCode = "+98",
                Code = "IR"
            },
            new CountryModel
            {
                Name = "Iraq",
                CallingCode = "+964",
                Code = "IQ"
            },
            new CountryModel
            {
                Name = "Ireland",
                CallingCode = "+353",
                Code = "IE"
            },
            new CountryModel
            {
                Name = "Isle of Man",
                CallingCode = "+44",
                Code = "IM"
            },
            new CountryModel
            {
                Name = "Israel",
                CallingCode = "+972",
                Code = "IL"
            },
            new CountryModel
            {
                Name = "Italy",
                CallingCode = "+39",
                Code = "IT"
            },
            new CountryModel
            {
                Name = "Jamaica",
                CallingCode = "+1876",
                Code = "JM"
            },
            new CountryModel
            {
                Name = "Japan",
                CallingCode = "+81",
                Code = "JP"
            },
            new CountryModel
            {
                Name = "Jersey",
                CallingCode = "+44",
                Code = "JE"
            },
            new CountryModel
            {
                Name = "Jordan",
                CallingCode = "+962",
                Code = "JO"
            },
            new CountryModel
            {
                Name = "Kazakhstan",
                CallingCode = "+77",
                Code = "KZ"
            },
            new CountryModel
            {
                Name = "Kenya",
                CallingCode = "+254",
                Code = "KE"
            },
            new CountryModel
            {
                Name = "Kiribati",
                CallingCode = "+686",
                Code = "KI"
            },
            new CountryModel
            {
                Name = "Korea, Democratic People's Republic of Korea",
                CallingCode = "+850",
                Code = "KP"
            },
            new CountryModel
            {
                Name = "Korea, Republic of South Korea",
                CallingCode = "+82",
                Code = "KR"
            },
            new CountryModel
            {
                Name = "Kosovo, Republic of Kosovo",
                CallingCode = "+383",
                Code = "XK"
            },
            new CountryModel
            {
                Name = "Kuwait",
                CallingCode = "+965",
                Code = "KW"
            },
            new CountryModel
            {
                Name = "Kyrgyzstan",
                CallingCode = "+996",
                Code = "KG"
            },
            new CountryModel
            {
                Name = "Laos",
                CallingCode = "+856",
                Code = "LA"
            },
            new CountryModel
            {
                Name = "Latvia",
                CallingCode = "+371",
                Code = "LV"
            },
            new CountryModel
            {
                Name = "Lebanon",
                CallingCode = "+961",
                Code = "LB"
            },
            new CountryModel
            {
                Name = "Lesotho",
                CallingCode = "+266",
                Code = "LS"
            },
            new CountryModel
            {
                Name = "Liberia",
                CallingCode = "+231",
                Code = "LR"
            },
            new CountryModel
            {
                Name = "Libyan Arab Jamahiriya",
                CallingCode = "+218",
                Code = "LY"
            },
            new CountryModel
            {
                Name = "Liechtenstein",
                CallingCode = "+423",
                Code = "LI"
            },
            new CountryModel
            {
                Name = "Lithuania",
                CallingCode = "+370",
                Code = "LT"
            },
            new CountryModel
            {
                Name = "Luxembourg",
                CallingCode = "+352",
                Code = "LU"
            },
            new CountryModel
            {
                Name = "Macao",
                CallingCode = "+853",
                Code = "MO"
            },
            new CountryModel
            {
                Name = "Macedonia",
                CallingCode = "+389",
                Code = "MK"
            },
            new CountryModel
            {
                Name = "Madagascar",
                CallingCode = "+261",
                Code = "MG"
            },
            new CountryModel
            {
                Name = "Malawi",
                CallingCode = "+265",
                Code = "MW"
            },
            new CountryModel
            {
                Name = "Malaysia",
                CallingCode = "+60",
                Code = "MY"
            },
            new CountryModel
            {
                Name = "Maldives",
                CallingCode = "+960",
                Code = "MV"
            },
            new CountryModel
            {
                Name = "Mali",
                CallingCode = "+223",
                Code = "ML"
            },
            new CountryModel
            {
                Name = "Malta",
                CallingCode = "+356",
                Code = "MT"
            },
            new CountryModel
            {
                Name = "Marshall Islands",
                CallingCode = "+692",
                Code = "MH"
            },
            new CountryModel
            {
                Name = "Martinique",
                CallingCode = "+596",
                Code = "MQ"
            },
            new CountryModel
            {
                Name = "Mauritania",
                CallingCode = "+222",
                Code = "MR"
            },
            new CountryModel
            {
                Name = "Mauritius",
                CallingCode = "+230",
                Code = "MU"
            },
            new CountryModel
            {
                Name = "Mayotte",
                CallingCode = "+262",
                Code = "YT"
            },
            new CountryModel
            {
                Name = "Mexico",
                CallingCode = "+52",
                Code = "MX"
            },
            new CountryModel
            {
                Name = "Micronesia, Federated States of Micronesia",
                CallingCode = "+691",
                Code = "FM"
            },
            new CountryModel
            {
                Name = "Moldova",
                CallingCode = "+373",
                Code = "MD"
            },
            new CountryModel
            {
                Name = "Monaco",
                CallingCode = "+377",
                Code = "MC"
            },
            new CountryModel
            {
                Name = "Mongolia",
                CallingCode = "+976",
                Code = "MN"
            },
            new CountryModel
            {
                Name = "Montenegro",
                CallingCode = "+382",
                Code = "ME"
            },
            new CountryModel
            {
                Name = "Montserrat",
                CallingCode = "+1664",
                Code = "MS"
            },
            new CountryModel
            {
                Name = "Morocco",
                CallingCode = "+212",
                Code = "MA"
            },
            new CountryModel
            {
                Name = "Mozambique",
                CallingCode = "+258",
                Code = "MZ"
            },
            new CountryModel
            {
                Name = "Myanmar",
                CallingCode = "+95",
                Code = "MM"
            },
            new CountryModel
            {
                Name = "Namibia",
                CallingCode = "+264",
                Code = "NA"
            },
            new CountryModel
            {
                Name = "Nauru",
                CallingCode = "+674",
                Code = "NR"
            },
            new CountryModel
            {
                Name = "Nepal",
                CallingCode = "+977",
                Code = "NP"
            },
            new CountryModel
            {
                Name = "Netherlands",
                CallingCode = "+31",
                Code = "NL"
            },
            new CountryModel
            {
                Name = "Netherlands Antilles",
                CallingCode = "+599",
                Code = "AN"
            },
            new CountryModel
            {
                Name = "New Caledonia",
                CallingCode = "+687",
                Code = "NC"
            },
            new CountryModel
            {
                Name = "New Zealand",
                CallingCode = "+64",
                Code = "NZ"
            },
            new CountryModel
            {
                Name = "Nicaragua",
                CallingCode = "+505",
                Code = "NI"
            },
            new CountryModel
            {
                Name = "Niger",
                CallingCode = "+227",
                Code = "NE"
            },
            new CountryModel
            {
                Name = "Nigeria",
                CallingCode = "+234",
                Code = "NG"
            },
            new CountryModel
            {
                Name = "Niue",
                CallingCode = "+683",
                Code = "NU"
            },
            new CountryModel
            {
                Name = "Norfolk Island",
                CallingCode = "+672",
                Code = "NF"
            },
            new CountryModel
            {
                Name = "Northern Mariana Islands",
                CallingCode = "+1670",
                Code = "MP"
            },
            new CountryModel
            {
                Name = "Norway",
                CallingCode = "+47",
                Code = "NO"
            },
            new CountryModel
            {
                Name = "Oman",
                CallingCode = "+968",
                Code = "OM"
            },
            new CountryModel
            {
                Name = "Pakistan",
                CallingCode = "+92",
                Code = "PK"
            },
            new CountryModel
            {
                Name = "Palau",
                CallingCode = "+680",
                Code = "PW"
            },
            new CountryModel
            {
                Name = "Palestinian Territory, Occupied",
                CallingCode = "+970",
                Code = "PS"
            },
            new CountryModel
            {
                Name = "Panama",
                CallingCode = "+507",
                Code = "PA"
            },
            new CountryModel
            {
                Name = "Papua New Guinea",
                CallingCode = "+675",
                Code = "PG"
            },
            new CountryModel
            {
                Name = "Paraguay",
                CallingCode = "+595",
                Code = "PY"
            },
            new CountryModel
            {
                Name = "Peru",
                CallingCode = "+51",
                Code = "PE"
            },
            new CountryModel
            {
                Name = "Philippines",
                CallingCode = "+63",
                Code = "PH"
            },
            new CountryModel
            {
                Name = "Pitcairn",
                CallingCode = "+870",
                Code = "PN"
            },
            new CountryModel
            {
                Name = "Poland",
                CallingCode = "+48",
                Code = "PL"
            },
            new CountryModel
            {
                Name = "Portugal",
                CallingCode = "+351",
                Code = "PT"
            },
            new CountryModel
            {
                Name = "Puerto Rico",
                CallingCode = "+1939",
                Code = "PR"
            },
            new CountryModel
            {
                Name = "Qatar",
                CallingCode = "+974",
                Code = "QA"
            },
            new CountryModel
            {
                Name = "Romania",
                CallingCode = "+40",
                Code = "RO"
            },
            new CountryModel
            {
                Name = "Russia",
                CallingCode = "+7",
                Code = "RU"
            },
            new CountryModel
            {
                Name = "Rwanda",
                CallingCode = "+250",
                Code = "RW"
            },
            new CountryModel
            {
                Name = "Reunion",
                CallingCode = "+262",
                Code = "RE"
            },
            new CountryModel
            {
                Name = "Saint Barthelemy",
                CallingCode = "+590",
                Code = "BL"
            },
            new CountryModel
            {
                Name = "Saint Helena, Ascension and Tristan Da Cunha",
                CallingCode = "+290",
                Code = "SH"
            },
            new CountryModel
            {
                Name = "Saint Kitts and Nevis",
                CallingCode = "+1869",
                Code = "KN"
            },
            new CountryModel
            {
                Name = "Saint Lucia",
                CallingCode = "+1758",
                Code = "LC"
            },
            new CountryModel
            {
                Name = "Saint Martin",
                CallingCode = "+590",
                Code = "MF"
            },
            new CountryModel
            {
                Name = "Saint Pierre and Miquelon",
                CallingCode = "+508",
                Code = "PM"
            },
            new CountryModel
            {
                Name = "Saint Vincent and the Grenadines",
                CallingCode = "+1784",
                Code = "VC"
            },
            new CountryModel
            {
                Name = "Samoa",
                CallingCode = "+685",
                Code = "WS"
            },
            new CountryModel
            {
                Name = "San Marino",
                CallingCode = "+378",
                Code = "SM"
            },
            new CountryModel
            {
                Name = "Sao Tome and Principe",
                CallingCode = "+239",
                Code = "ST"
            },
            new CountryModel
            {
                Name = "Saudi Arabia",
                CallingCode = "+966",
                Code = "SA"
            },
            new CountryModel
            {
                Name = "Senegal",
                CallingCode = "+221",
                Code = "SN"
            },
            new CountryModel
            {
                Name = "Serbia",
                CallingCode = "+381",
                Code = "RS"
            },
            new CountryModel
            {
                Name = "Seychelles",
                CallingCode = "+248",
                Code = "SC"
            },
            new CountryModel
            {
                Name = "Sierra Leone",
                CallingCode = "+232",
                Code = "SL"
            },
            new CountryModel
            {
                Name = "Singapore",
                CallingCode = "+65",
                Code = "SG"
            },
            new CountryModel
            {
                Name = "Sint Maarten",
                CallingCode = "+1721",
                Code = "SX"
            },
            new CountryModel
            {
                Name = "Slovakia",
                CallingCode = "+421",
                Code = "SK"
            },
            new CountryModel
            {
                Name = "Slovenia",
                CallingCode = "+386",
                Code = "SI"
            },
            new CountryModel
            {
                Name = "Solomon Islands",
                CallingCode = "+677",
                Code = "SB"
            },
            new CountryModel
            {
                Name = "Somalia",
                CallingCode = "+252",
                Code = "SO"
            },
            new CountryModel
            {
                Name = "South Africa",
                CallingCode = "+27",
                Code = "ZA"
            },
            new CountryModel
            {
                Name = "South Sudan",
                CallingCode = "+211",
                Code = "SS"
            },
            new CountryModel
            {
                Name = "South Georgia and the South Sandwich Islands",
                CallingCode = "+500",
                Code = "GS"
            },
            new CountryModel
            {
                Name = "Spain",
                CallingCode = "+34",
                Code = "ES"
            },
            new CountryModel
            {
                Name = "Sri Lanka",
                CallingCode = "+94",
                Code = "LK"
            },
            new CountryModel
            {
                Name = "Sudan",
                CallingCode = "+249",
                Code = "SD"
            },
            new CountryModel
            {
                Name = "Suriname",
                CallingCode = "+597",
                Code = "SR"
            },
            new CountryModel
            {
                Name = "Svalbard and Jan Mayen",
                CallingCode = "+47",
                Code = "SJ"
            },
            new CountryModel
            {
                Name = "Swaziland",
                CallingCode = "+268",
                Code = "SZ"
            },
            new CountryModel
            {
                Name = "Sweden",
                CallingCode = "+46",
                Code = "SE"
            },
            new CountryModel
            {
                Name = "Switzerland",
                CallingCode = "+41",
                Code = "CH"
            },
            new CountryModel
            {
                Name = "Syrian Arab Republic",
                CallingCode = "+963",
                Code = "SY"
            },
            new CountryModel
            {
                Name = "Taiwan",
                CallingCode = "+886",
                Code = "TW"
            },
            new CountryModel
            {
                Name = "Tajikistan",
                CallingCode = "+992",
                Code = "TJ"
            },
            new CountryModel
            {
                Name = "Tanzania, United Republic of Tanzania",
                CallingCode = "+255",
                Code = "TZ"
            },
            new CountryModel
            {
                Name = "Thailand",
                CallingCode = "+66",
                Code = "TH"
            },
            new CountryModel
            {
                Name = "Timor-Leste",
                CallingCode = "+670",
                Code = "TL"
            },
            new CountryModel
            {
                Name = "Togo",
                CallingCode = "+228",
                Code = "TG"
            },
            new CountryModel
            {
                Name = "Tokelau",
                CallingCode = "+690",
                Code = "TK"
            },
            new CountryModel
            {
                Name = "Tonga",
                CallingCode = "+676",
                Code = "TO"
            },
            new CountryModel
            {
                Name = "Trinidad and Tobago",
                CallingCode = "+1868",
                Code = "TT"
            },
            new CountryModel
            {
                Name = "Tunisia",
                CallingCode = "+216",
                Code = "TN"
            },
            new CountryModel
            {
                Name = "Turkey",
                CallingCode = "+90",
                Code = "TR"
            },
            new CountryModel
            {
                Name = "Turkmenistan",
                CallingCode = "+993",
                Code = "TM"
            },
            new CountryModel
            {
                Name = "Turks and Caicos Islands",
                CallingCode = "+1649",
                Code = "TC"
            },
            new CountryModel
            {
                Name = "Tuvalu",
                CallingCode = "+688",
                Code = "TV"
            },
            new CountryModel
            {
                Name = "Uganda",
                CallingCode = "+256",
                Code = "UG"
            },
            new CountryModel
            {
                Name = "Ukraine",
                CallingCode = "+380",
                Code = "UA"
            },
            new CountryModel
            {
                Name = "United Arab Emirates",
                CallingCode = "+971",
                Code = "AE"
            },
            new CountryModel
            {
                Name = "United Kingdom",
                CallingCode = "+44",
                Code = "GB"
            },
            new CountryModel
            {
                Name = "United States",
                CallingCode = "+1",
                Code = "US"
            },
            new CountryModel
            {
                Name = "United States Minor Outlying Islands",
                CallingCode = "+1581",
                Code = "UM"
            },
            new CountryModel
            {
                Name = "Uruguay",
                CallingCode = "+598",
                Code = "UY"
            },
            new CountryModel
            {
                Name = "Uzbekistan",
                CallingCode = "+998",
                Code = "UZ"
            },
            new CountryModel
            {
                Name = "Vanuatu",
                CallingCode = "+678",
                Code = "VU"
            },
            new CountryModel
            {
                Name = "Venezuela, Bolivarian Republic of Venezuela",
                CallingCode = "+58",
                Code = "VE"
            },
            new CountryModel
            {
                Name = "Viet Nam",
                CallingCode = "+84",
                Code = "VN"
            },
            new CountryModel
            {
                Name = "Virgin Islands, British",
                CallingCode = "+1284",
                Code = "VG"
            },
            new CountryModel
            {
                Name = "Virgin Islands, U.S.",
                CallingCode = "+1340",
                Code = "VI"
            },
            new CountryModel
            {
                Name = "Wallis and Futuna",
                CallingCode = "+681",
                Code = "WF"
            },
            new CountryModel
            {
                Name = "Western Sahara",
                CallingCode = "+212",
                Code = "EH"
            },
            new CountryModel
            {
                Name = "Yemen",
                CallingCode = "+967",
                Code = "YE"
            },
            new CountryModel
            {
                Name = "Zambia",
                CallingCode = "+260",
                Code = "ZM"
            },
            new CountryModel
            {
                Name = "Zimbabwe",
                CallingCode = "+263",
                Code = "ZW"
            }
        };
    }
}