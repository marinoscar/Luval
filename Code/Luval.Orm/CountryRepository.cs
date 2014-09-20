using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Common;
using Luval.Orm.Models;

namespace Luval.Orm
{
    public class CountryRepository
    {
        public CountryRepository()
        {
            Countries = new List<Country>(240);
            LoadList();
        }


        private static CountryRepository _instance;
        public static CountryRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    var provider = ObjectCacheProvider.GetProvider<string, CountryRepository>("CountryRepoCache");
                    var repo = provider.GetCacheItem("repo", i => new CountryRepository());
                    _instance = repo;
                }
                return _instance;
            }
        }

        public Country GetCountryByCaption(string caption)
        {
            return GetCountry(i => i.Caption == caption);
        }

        public Country GetCountryByName(string name)
        {
            return GetCountry(i => i.Name == name);
        }

        public Country GetCountryByLongCode(string longCode)
        {
            return GetCountry(i => i.LongCode == longCode);
        }

        public Country GetCountryByShortCode(string shortCode)
        {
            return GetCountry(i => i.ShortCode == shortCode);
        }

        public Country GetCountry(Func<Country, bool> predicate)
        {
            return Countries.FirstOrDefault(predicate);
        }

        public IEnumerable<Country> Countries { get; private set; }


        private void LoadList()
        {
            Countries = new List<Country>(240)
                {
                    new Country{ ShortCode = "AD", LongCode = "AND", Name = "ANDORRA", Caption = "Andorra", NumericCode = 20},
                    new Country{ ShortCode = "AE", LongCode = "ARE", Name = "UNITED ARAB EMIRATES", Caption = "United Arab Emirates", NumericCode = 784},
                    new Country{ ShortCode = "AF", LongCode = "AFG", Name = "AFGHANISTAN", Caption = "Afghanistan", NumericCode = 4},
                    new Country{ ShortCode = "AG", LongCode = "ATG", Name = "ANTIGUA AND BARBUDA", Caption = "Antigua and Barbuda", NumericCode = 28},
                    new Country{ ShortCode = "AI", LongCode = "AIA", Name = "ANGUILLA", Caption = "Anguilla", NumericCode = 660},
                    new Country{ ShortCode = "AL", LongCode = "ALB", Name = "ALBANIA", Caption = "Albania", NumericCode = 8},
                    new Country{ ShortCode = "AM", LongCode = "ARM", Name = "ARMENIA", Caption = "Armenia", NumericCode = 51},
                    new Country{ ShortCode = "AN", LongCode = "ANT", Name = "NETHERLANDS ANTILLES", Caption = "Netherlands Antilles", NumericCode = 530},
                    new Country{ ShortCode = "AO", LongCode = "AGO", Name = "ANGOLA", Caption = "Angola", NumericCode = 24},
                    new Country{ ShortCode = "AQ", LongCode = "", Name = "ANTARCTICA", Caption = "Antarctica", NumericCode = 0},
                    new Country{ ShortCode = "AR", LongCode = "ARG", Name = "ARGENTINA", Caption = "Argentina", NumericCode = 32},
                    new Country{ ShortCode = "AS", LongCode = "ASM", Name = "AMERICAN SAMOA", Caption = "American Samoa", NumericCode = 16},
                    new Country{ ShortCode = "AT", LongCode = "AUT", Name = "AUSTRIA", Caption = "Austria", NumericCode = 40},
                    new Country{ ShortCode = "AU", LongCode = "AUS", Name = "AUSTRALIA", Caption = "Australia", NumericCode = 36},
                    new Country{ ShortCode = "AW", LongCode = "ABW", Name = "ARUBA", Caption = "Aruba", NumericCode = 533},
                    new Country{ ShortCode = "AZ", LongCode = "AZE", Name = "AZERBAIJAN", Caption = "Azerbaijan", NumericCode = 31},
                    new Country{ ShortCode = "BA", LongCode = "BIH", Name = "BOSNIA AND HERZEGOVINA", Caption = "Bosnia and Herzegovina", NumericCode = 70},
                    new Country{ ShortCode = "BB", LongCode = "BRB", Name = "BARBADOS", Caption = "Barbados", NumericCode = 52},
                    new Country{ ShortCode = "BD", LongCode = "BGD", Name = "BANGLADESH", Caption = "Bangladesh", NumericCode = 50},
                    new Country{ ShortCode = "BE", LongCode = "BEL", Name = "BELGIUM", Caption = "Belgium", NumericCode = 56},
                    new Country{ ShortCode = "BF", LongCode = "BFA", Name = "BURKINA FASO", Caption = "Burkina Faso", NumericCode = 854},
                    new Country{ ShortCode = "BG", LongCode = "BGR", Name = "BULGARIA", Caption = "Bulgaria", NumericCode = 100},
                    new Country{ ShortCode = "BH", LongCode = "BHR", Name = "BAHRAIN", Caption = "Bahrain", NumericCode = 48},
                    new Country{ ShortCode = "BI", LongCode = "BDI", Name = "BURUNDI", Caption = "Burundi", NumericCode = 108},
                    new Country{ ShortCode = "BJ", LongCode = "BEN", Name = "BENIN", Caption = "Benin", NumericCode = 204},
                    new Country{ ShortCode = "BM", LongCode = "BMU", Name = "BERMUDA", Caption = "Bermuda", NumericCode = 60},
                    new Country{ ShortCode = "BN", LongCode = "BRN", Name = "BRUNEI DARUSSALAM", Caption = "Brunei Darussalam", NumericCode = 96},
                    new Country{ ShortCode = "BO", LongCode = "BOL", Name = "BOLIVIA", Caption = "Bolivia", NumericCode = 68},
                    new Country{ ShortCode = "BR", LongCode = "BRA", Name = "BRAZIL", Caption = "Brazil", NumericCode = 76},
                    new Country{ ShortCode = "BS", LongCode = "BHS", Name = "BAHAMAS", Caption = "Bahamas", NumericCode = 44},
                    new Country{ ShortCode = "BT", LongCode = "BTN", Name = "BHUTAN", Caption = "Bhutan", NumericCode = 64},
                    new Country{ ShortCode = "BV", LongCode = "", Name = "BOUVET ISLAND", Caption = "Bouvet Island", NumericCode = 0},
                    new Country{ ShortCode = "BW", LongCode = "BWA", Name = "BOTSWANA", Caption = "Botswana", NumericCode = 72},
                    new Country{ ShortCode = "BY", LongCode = "BLR", Name = "BELARUS", Caption = "Belarus", NumericCode = 112},
                    new Country{ ShortCode = "BZ", LongCode = "BLZ", Name = "BELIZE", Caption = "Belize", NumericCode = 84},
                    new Country{ ShortCode = "CA", LongCode = "CAN", Name = "CANADA", Caption = "Canada", NumericCode = 124},
                    new Country{ ShortCode = "CC", LongCode = "", Name = "COCOS (KEELING) ISLANDS", Caption = "Cocos (Keeling) Islands", NumericCode = 0},
                    new Country{ ShortCode = "CD", LongCode = "COD", Name = "CONGO, THE DEMOCRATIC REPUBLIC OF THE", Caption = "Congo, the Democratic Republic of the", NumericCode = 180},
                    new Country{ ShortCode = "CF", LongCode = "CAF", Name = "CENTRAL AFRICAN REPUBLIC", Caption = "Central African Republic", NumericCode = 140},
                    new Country{ ShortCode = "CG", LongCode = "COG", Name = "CONGO", Caption = "Congo", NumericCode = 178},
                    new Country{ ShortCode = "CH", LongCode = "CHE", Name = "SWITZERLAND", Caption = "Switzerland", NumericCode = 756},
                    new Country{ ShortCode = "CI", LongCode = "CIV", Name = "COTE D'IVOIRE", Caption = "Cote D'Ivoire", NumericCode = 384},
                    new Country{ ShortCode = "CK", LongCode = "COK", Name = "COOK ISLANDS", Caption = "Cook Islands", NumericCode = 184},
                    new Country{ ShortCode = "CL", LongCode = "CHL", Name = "CHILE", Caption = "Chile", NumericCode = 152},
                    new Country{ ShortCode = "CM", LongCode = "CMR", Name = "CAMEROON", Caption = "Cameroon", NumericCode = 120},
                    new Country{ ShortCode = "CN", LongCode = "CHN", Name = "CHINA", Caption = "China", NumericCode = 156},
                    new Country{ ShortCode = "CO", LongCode = "COL", Name = "COLOMBIA", Caption = "Colombia", NumericCode = 170},
                    new Country{ ShortCode = "CR", LongCode = "CRI", Name = "COSTA RICA", Caption = "Costa Rica", NumericCode = 188},
                    new Country{ ShortCode = "CS", LongCode = "", Name = "SERBIA AND MONTENEGRO", Caption = "Serbia and Montenegro", NumericCode = 0},
                    new Country{ ShortCode = "CU", LongCode = "CUB", Name = "CUBA", Caption = "Cuba", NumericCode = 192},
                    new Country{ ShortCode = "CV", LongCode = "CPV", Name = "CAPE VERDE", Caption = "Cape Verde", NumericCode = 132},
                    new Country{ ShortCode = "CX", LongCode = "", Name = "CHRISTMAS ISLAND", Caption = "Christmas Island", NumericCode = 0},
                    new Country{ ShortCode = "CY", LongCode = "CYP", Name = "CYPRUS", Caption = "Cyprus", NumericCode = 196},
                    new Country{ ShortCode = "CZ", LongCode = "CZE", Name = "CZECH REPUBLIC", Caption = "Czech Republic", NumericCode = 203},
                    new Country{ ShortCode = "DE", LongCode = "DEU", Name = "GERMANY", Caption = "Germany", NumericCode = 276},
                    new Country{ ShortCode = "DJ", LongCode = "DJI", Name = "DJIBOUTI", Caption = "Djibouti", NumericCode = 262},
                    new Country{ ShortCode = "DK", LongCode = "DNK", Name = "DENMARK", Caption = "Denmark", NumericCode = 208},
                    new Country{ ShortCode = "DM", LongCode = "DMA", Name = "DOMINICA", Caption = "Dominica", NumericCode = 212},
                    new Country{ ShortCode = "DO", LongCode = "DOM", Name = "DOMINICAN REPUBLIC", Caption = "Dominican Republic", NumericCode = 214},
                    new Country{ ShortCode = "DZ", LongCode = "DZA", Name = "ALGERIA", Caption = "Algeria", NumericCode = 12},
                    new Country{ ShortCode = "EC", LongCode = "ECU", Name = "ECUADOR", Caption = "Ecuador", NumericCode = 218},
                    new Country{ ShortCode = "EE", LongCode = "EST", Name = "ESTONIA", Caption = "Estonia", NumericCode = 233},
                    new Country{ ShortCode = "EG", LongCode = "EGY", Name = "EGYPT", Caption = "Egypt", NumericCode = 818},
                    new Country{ ShortCode = "EH", LongCode = "ESH", Name = "WESTERN SAHARA", Caption = "Western Sahara", NumericCode = 732},
                    new Country{ ShortCode = "ER", LongCode = "ERI", Name = "ERITREA", Caption = "Eritrea", NumericCode = 232},
                    new Country{ ShortCode = "ES", LongCode = "ESP", Name = "SPAIN", Caption = "Spain", NumericCode = 724},
                    new Country{ ShortCode = "ET", LongCode = "ETH", Name = "ETHIOPIA", Caption = "Ethiopia", NumericCode = 231},
                    new Country{ ShortCode = "FI", LongCode = "FIN", Name = "FINLAND", Caption = "Finland", NumericCode = 246},
                    new Country{ ShortCode = "FJ", LongCode = "FJI", Name = "FIJI", Caption = "Fiji", NumericCode = 242},
                    new Country{ ShortCode = "FK", LongCode = "FLK", Name = "FALKLAND ISLANDS (MALVINAS)", Caption = "Falkland Islands (Malvinas)", NumericCode = 238},
                    new Country{ ShortCode = "FM", LongCode = "FSM", Name = "MICRONESIA, FEDERATED STATES OF", Caption = "Micronesia, Federated States of", NumericCode = 583},
                    new Country{ ShortCode = "FO", LongCode = "FRO", Name = "FAROE ISLANDS", Caption = "Faroe Islands", NumericCode = 234},
                    new Country{ ShortCode = "FR", LongCode = "FRA", Name = "FRANCE", Caption = "France", NumericCode = 250},
                    new Country{ ShortCode = "GA", LongCode = "GAB", Name = "GABON", Caption = "Gabon", NumericCode = 266},
                    new Country{ ShortCode = "GB", LongCode = "GBR", Name = "UNITED KINGDOM", Caption = "United Kingdom", NumericCode = 826},
                    new Country{ ShortCode = "GD", LongCode = "GRD", Name = "GRENADA", Caption = "Grenada", NumericCode = 308},
                    new Country{ ShortCode = "GE", LongCode = "GEO", Name = "GEORGIA", Caption = "Georgia", NumericCode = 268},
                    new Country{ ShortCode = "GF", LongCode = "GUF", Name = "FRENCH GUIANA", Caption = "French Guiana", NumericCode = 254},
                    new Country{ ShortCode = "GH", LongCode = "GHA", Name = "GHANA", Caption = "Ghana", NumericCode = 288},
                    new Country{ ShortCode = "GI", LongCode = "GIB", Name = "GIBRALTAR", Caption = "Gibraltar", NumericCode = 292},
                    new Country{ ShortCode = "GL", LongCode = "GRL", Name = "GREENLAND", Caption = "Greenland", NumericCode = 304},
                    new Country{ ShortCode = "GM", LongCode = "GMB", Name = "GAMBIA", Caption = "Gambia", NumericCode = 270},
                    new Country{ ShortCode = "GN", LongCode = "GIN", Name = "GUINEA", Caption = "Guinea", NumericCode = 324},
                    new Country{ ShortCode = "GP", LongCode = "GLP", Name = "GUADELOUPE", Caption = "Guadeloupe", NumericCode = 312},
                    new Country{ ShortCode = "GQ", LongCode = "GNQ", Name = "EQUATORIAL GUINEA", Caption = "Equatorial Guinea", NumericCode = 226},
                    new Country{ ShortCode = "GR", LongCode = "GRC", Name = "GREECE", Caption = "Greece", NumericCode = 300},
                    new Country{ ShortCode = "GS", LongCode = "", Name = "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS", Caption = "South Georgia and the South Sandwich Islands", NumericCode = 0},
                    new Country{ ShortCode = "GT", LongCode = "GTM", Name = "GUATEMALA", Caption = "Guatemala", NumericCode = 320},
                    new Country{ ShortCode = "GU", LongCode = "GUM", Name = "GUAM", Caption = "Guam", NumericCode = 316},
                    new Country{ ShortCode = "GW", LongCode = "GNB", Name = "GUINEA-BISSAU", Caption = "Guinea-Bissau", NumericCode = 624},
                    new Country{ ShortCode = "GY", LongCode = "GUY", Name = "GUYANA", Caption = "Guyana", NumericCode = 328},
                    new Country{ ShortCode = "HK", LongCode = "HKG", Name = "HONG KONG", Caption = "Hong Kong", NumericCode = 344},
                    new Country{ ShortCode = "HM", LongCode = "", Name = "HEARD ISLAND AND MCDONALD ISLANDS", Caption = "Heard Island and Mcdonald Islands", NumericCode = 0},
                    new Country{ ShortCode = "HN", LongCode = "HND", Name = "HONDURAS", Caption = "Honduras", NumericCode = 340},
                    new Country{ ShortCode = "HR", LongCode = "HRV", Name = "CROATIA", Caption = "Croatia", NumericCode = 191},
                    new Country{ ShortCode = "HT", LongCode = "HTI", Name = "HAITI", Caption = "Haiti", NumericCode = 332},
                    new Country{ ShortCode = "HU", LongCode = "HUN", Name = "HUNGARY", Caption = "Hungary", NumericCode = 348},
                    new Country{ ShortCode = "ID", LongCode = "IDN", Name = "INDONESIA", Caption = "Indonesia", NumericCode = 360},
                    new Country{ ShortCode = "IE", LongCode = "IRL", Name = "IRELAND", Caption = "Ireland", NumericCode = 372},
                    new Country{ ShortCode = "IL", LongCode = "ISR", Name = "ISRAEL", Caption = "Israel", NumericCode = 376},
                    new Country{ ShortCode = "IN", LongCode = "IND", Name = "INDIA", Caption = "India", NumericCode = 356},
                    new Country{ ShortCode = "IO", LongCode = "", Name = "BRITISH INDIAN OCEAN TERRITORY", Caption = "British Indian Ocean Territory", NumericCode = 0},
                    new Country{ ShortCode = "IQ", LongCode = "IRQ", Name = "IRAQ", Caption = "Iraq", NumericCode = 368},
                    new Country{ ShortCode = "IR", LongCode = "IRN", Name = "IRAN, ISLAMIC REPUBLIC OF", Caption = "Iran, Islamic Republic of", NumericCode = 364},
                    new Country{ ShortCode = "IS", LongCode = "ISL", Name = "ICELAND", Caption = "Iceland", NumericCode = 352},
                    new Country{ ShortCode = "IT", LongCode = "ITA", Name = "ITALY", Caption = "Italy", NumericCode = 380},
                    new Country{ ShortCode = "JM", LongCode = "JAM", Name = "JAMAICA", Caption = "Jamaica", NumericCode = 388},
                    new Country{ ShortCode = "JO", LongCode = "JOR", Name = "JORDAN", Caption = "Jordan", NumericCode = 400},
                    new Country{ ShortCode = "JP", LongCode = "JPN", Name = "JAPAN", Caption = "Japan", NumericCode = 392},
                    new Country{ ShortCode = "KE", LongCode = "KEN", Name = "KENYA", Caption = "Kenya", NumericCode = 404},
                    new Country{ ShortCode = "KG", LongCode = "KGZ", Name = "KYRGYZSTAN", Caption = "Kyrgyzstan", NumericCode = 417},
                    new Country{ ShortCode = "KH", LongCode = "KHM", Name = "CAMBODIA", Caption = "Cambodia", NumericCode = 116},
                    new Country{ ShortCode = "KI", LongCode = "KIR", Name = "KIRIBATI", Caption = "Kiribati", NumericCode = 296},
                    new Country{ ShortCode = "KM", LongCode = "COM", Name = "COMOROS", Caption = "Comoros", NumericCode = 174},
                    new Country{ ShortCode = "KN", LongCode = "KNA", Name = "SAINT KITTS AND NEVIS", Caption = "Saint Kitts and Nevis", NumericCode = 659},
                    new Country{ ShortCode = "KP", LongCode = "PRK", Name = "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF", Caption = "Korea, Democratic People's Republic of", NumericCode = 408},
                    new Country{ ShortCode = "KR", LongCode = "KOR", Name = "KOREA, REPUBLIC OF", Caption = "Korea, Republic of", NumericCode = 410},
                    new Country{ ShortCode = "KW", LongCode = "KWT", Name = "KUWAIT", Caption = "Kuwait", NumericCode = 414},
                    new Country{ ShortCode = "KY", LongCode = "CYM", Name = "CAYMAN ISLANDS", Caption = "Cayman Islands", NumericCode = 136},
                    new Country{ ShortCode = "KZ", LongCode = "KAZ", Name = "KAZAKHSTAN", Caption = "Kazakhstan", NumericCode = 398},
                    new Country{ ShortCode = "LA", LongCode = "LAO", Name = "LAO PEOPLE'S DEMOCRATIC REPUBLIC", Caption = "Lao People's Democratic Republic", NumericCode = 418},
                    new Country{ ShortCode = "LB", LongCode = "LBN", Name = "LEBANON", Caption = "Lebanon", NumericCode = 422},
                    new Country{ ShortCode = "LC", LongCode = "LCA", Name = "SAINT LUCIA", Caption = "Saint Lucia", NumericCode = 662},
                    new Country{ ShortCode = "LI", LongCode = "LIE", Name = "LIECHTENSTEIN", Caption = "Liechtenstein", NumericCode = 438},
                    new Country{ ShortCode = "LK", LongCode = "LKA", Name = "SRI LANKA", Caption = "Sri Lanka", NumericCode = 144},
                    new Country{ ShortCode = "LR", LongCode = "LBR", Name = "LIBERIA", Caption = "Liberia", NumericCode = 430},
                    new Country{ ShortCode = "LS", LongCode = "LSO", Name = "LESOTHO", Caption = "Lesotho", NumericCode = 426},
                    new Country{ ShortCode = "LT", LongCode = "LTU", Name = "LITHUANIA", Caption = "Lithuania", NumericCode = 440},
                    new Country{ ShortCode = "LU", LongCode = "LUX", Name = "LUXEMBOURG", Caption = "Luxembourg", NumericCode = 442},
                    new Country{ ShortCode = "LV", LongCode = "LVA", Name = "LATVIA", Caption = "Latvia", NumericCode = 428},
                    new Country{ ShortCode = "LY", LongCode = "LBY", Name = "LIBYAN ARAB JAMAHIRIYA", Caption = "Libyan Arab Jamahiriya", NumericCode = 434},
                    new Country{ ShortCode = "MA", LongCode = "MAR", Name = "MOROCCO", Caption = "Morocco", NumericCode = 504},
                    new Country{ ShortCode = "MC", LongCode = "MCO", Name = "MONACO", Caption = "Monaco", NumericCode = 492},
                    new Country{ ShortCode = "MD", LongCode = "MDA", Name = "MOLDOVA, REPUBLIC OF", Caption = "Moldova, Republic of", NumericCode = 498},
                    new Country{ ShortCode = "MG", LongCode = "MDG", Name = "MADAGASCAR", Caption = "Madagascar", NumericCode = 450},
                    new Country{ ShortCode = "MH", LongCode = "MHL", Name = "MARSHALL ISLANDS", Caption = "Marshall Islands", NumericCode = 584},
                    new Country{ ShortCode = "MK", LongCode = "MKD", Name = "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF", Caption = "Macedonia, the Former Yugoslav Republic of", NumericCode = 807},
                    new Country{ ShortCode = "ML", LongCode = "MLI", Name = "MALI", Caption = "Mali", NumericCode = 466},
                    new Country{ ShortCode = "MM", LongCode = "MMR", Name = "MYANMAR", Caption = "Myanmar", NumericCode = 104},
                    new Country{ ShortCode = "MN", LongCode = "MNG", Name = "MONGOLIA", Caption = "Mongolia", NumericCode = 496},
                    new Country{ ShortCode = "MO", LongCode = "MAC", Name = "MACAO", Caption = "Macao", NumericCode = 446},
                    new Country{ ShortCode = "MP", LongCode = "MNP", Name = "NORTHERN MARIANA ISLANDS", Caption = "Northern Mariana Islands", NumericCode = 580},
                    new Country{ ShortCode = "MQ", LongCode = "MTQ", Name = "MARTINIQUE", Caption = "Martinique", NumericCode = 474},
                    new Country{ ShortCode = "MR", LongCode = "MRT", Name = "MAURITANIA", Caption = "Mauritania", NumericCode = 478},
                    new Country{ ShortCode = "MS", LongCode = "MSR", Name = "MONTSERRAT", Caption = "Montserrat", NumericCode = 500},
                    new Country{ ShortCode = "MT", LongCode = "MLT", Name = "MALTA", Caption = "Malta", NumericCode = 470},
                    new Country{ ShortCode = "MU", LongCode = "MUS", Name = "MAURITIUS", Caption = "Mauritius", NumericCode = 480},
                    new Country{ ShortCode = "MV", LongCode = "MDV", Name = "MALDIVES", Caption = "Maldives", NumericCode = 462},
                    new Country{ ShortCode = "MW", LongCode = "MWI", Name = "MALAWI", Caption = "Malawi", NumericCode = 454},
                    new Country{ ShortCode = "MX", LongCode = "MEX", Name = "MEXICO", Caption = "Mexico", NumericCode = 484},
                    new Country{ ShortCode = "MY", LongCode = "MYS", Name = "MALAYSIA", Caption = "Malaysia", NumericCode = 458},
                    new Country{ ShortCode = "MZ", LongCode = "MOZ", Name = "MOZAMBIQUE", Caption = "Mozambique", NumericCode = 508},
                    new Country{ ShortCode = "NA", LongCode = "NAM", Name = "NAMIBIA", Caption = "Namibia", NumericCode = 516},
                    new Country{ ShortCode = "NC", LongCode = "NCL", Name = "NEW CALEDONIA", Caption = "New Caledonia", NumericCode = 540},
                    new Country{ ShortCode = "NE", LongCode = "NER", Name = "NIGER", Caption = "Niger", NumericCode = 562},
                    new Country{ ShortCode = "NF", LongCode = "NFK", Name = "NORFOLK ISLAND", Caption = "Norfolk Island", NumericCode = 574},
                    new Country{ ShortCode = "NG", LongCode = "NGA", Name = "NIGERIA", Caption = "Nigeria", NumericCode = 566},
                    new Country{ ShortCode = "NI", LongCode = "NIC", Name = "NICARAGUA", Caption = "Nicaragua", NumericCode = 558},
                    new Country{ ShortCode = "NL", LongCode = "NLD", Name = "NETHERLANDS", Caption = "Netherlands", NumericCode = 528},
                    new Country{ ShortCode = "NO", LongCode = "NOR", Name = "NORWAY", Caption = "Norway", NumericCode = 578},
                    new Country{ ShortCode = "NP", LongCode = "NPL", Name = "NEPAL", Caption = "Nepal", NumericCode = 524},
                    new Country{ ShortCode = "NR", LongCode = "NRU", Name = "NAURU", Caption = "Nauru", NumericCode = 520},
                    new Country{ ShortCode = "NU", LongCode = "NIU", Name = "NIUE", Caption = "Niue", NumericCode = 570},
                    new Country{ ShortCode = "NZ", LongCode = "NZL", Name = "NEW ZEALAND", Caption = "New Zealand", NumericCode = 554},
                    new Country{ ShortCode = "OM", LongCode = "OMN", Name = "OMAN", Caption = "Oman", NumericCode = 512},
                    new Country{ ShortCode = "PA", LongCode = "PAN", Name = "PANAMA", Caption = "Panama", NumericCode = 591},
                    new Country{ ShortCode = "PE", LongCode = "PER", Name = "PERU", Caption = "Peru", NumericCode = 604},
                    new Country{ ShortCode = "PF", LongCode = "PYF", Name = "FRENCH POLYNESIA", Caption = "French Polynesia", NumericCode = 258},
                    new Country{ ShortCode = "PG", LongCode = "PNG", Name = "PAPUA NEW GUINEA", Caption = "Papua New Guinea", NumericCode = 598},
                    new Country{ ShortCode = "PH", LongCode = "PHL", Name = "PHILIPPINES", Caption = "Philippines", NumericCode = 608},
                    new Country{ ShortCode = "PK", LongCode = "PAK", Name = "PAKISTAN", Caption = "Pakistan", NumericCode = 586},
                    new Country{ ShortCode = "PL", LongCode = "POL", Name = "POLAND", Caption = "Poland", NumericCode = 616},
                    new Country{ ShortCode = "PM", LongCode = "SPM", Name = "SAINT PIERRE AND MIQUELON", Caption = "Saint Pierre and Miquelon", NumericCode = 666},
                    new Country{ ShortCode = "PN", LongCode = "PCN", Name = "PITCAIRN", Caption = "Pitcairn", NumericCode = 612},
                    new Country{ ShortCode = "PR", LongCode = "PRI", Name = "PUERTO RICO", Caption = "Puerto Rico", NumericCode = 630},
                    new Country{ ShortCode = "PS", LongCode = "", Name = "PALESTINIAN TERRITORY, OCCUPIED", Caption = "Palestinian Territory, Occupied", NumericCode = 0},
                    new Country{ ShortCode = "PT", LongCode = "PRT", Name = "PORTUGAL", Caption = "Portugal", NumericCode = 620},
                    new Country{ ShortCode = "PW", LongCode = "PLW", Name = "PALAU", Caption = "Palau", NumericCode = 585},
                    new Country{ ShortCode = "PY", LongCode = "PRY", Name = "PARAGUAY", Caption = "Paraguay", NumericCode = 600},
                    new Country{ ShortCode = "QA", LongCode = "QAT", Name = "QATAR", Caption = "Qatar", NumericCode = 634},
                    new Country{ ShortCode = "RE", LongCode = "REU", Name = "REUNION", Caption = "Reunion", NumericCode = 638},
                    new Country{ ShortCode = "RO", LongCode = "ROM", Name = "ROMANIA", Caption = "Romania", NumericCode = 642},
                    new Country{ ShortCode = "RU", LongCode = "RUS", Name = "RUSSIAN FEDERATION", Caption = "Russian Federation", NumericCode = 643},
                    new Country{ ShortCode = "RW", LongCode = "RWA", Name = "RWANDA", Caption = "Rwanda", NumericCode = 646},
                    new Country{ ShortCode = "SA", LongCode = "SAU", Name = "SAUDI ARABIA", Caption = "Saudi Arabia", NumericCode = 682},
                    new Country{ ShortCode = "SB", LongCode = "SLB", Name = "SOLOMON ISLANDS", Caption = "Solomon Islands", NumericCode = 90},
                    new Country{ ShortCode = "SC", LongCode = "SYC", Name = "SEYCHELLES", Caption = "Seychelles", NumericCode = 690},
                    new Country{ ShortCode = "SD", LongCode = "SDN", Name = "SUDAN", Caption = "Sudan", NumericCode = 736},
                    new Country{ ShortCode = "SE", LongCode = "SWE", Name = "SWEDEN", Caption = "Sweden", NumericCode = 752},
                    new Country{ ShortCode = "SG", LongCode = "SGP", Name = "SINGAPORE", Caption = "Singapore", NumericCode = 702},
                    new Country{ ShortCode = "SH", LongCode = "SHN", Name = "SAINT HELENA", Caption = "Saint Helena", NumericCode = 654},
                    new Country{ ShortCode = "SI", LongCode = "SVN", Name = "SLOVENIA", Caption = "Slovenia", NumericCode = 705},
                    new Country{ ShortCode = "SJ", LongCode = "SJM", Name = "SVALBARD AND JAN MAYEN", Caption = "Svalbard and Jan Mayen", NumericCode = 744},
                    new Country{ ShortCode = "SK", LongCode = "SVK", Name = "SLOVAKIA", Caption = "Slovakia", NumericCode = 703},
                    new Country{ ShortCode = "SL", LongCode = "SLE", Name = "SIERRA LEONE", Caption = "Sierra Leone", NumericCode = 694},
                    new Country{ ShortCode = "SM", LongCode = "SMR", Name = "SAN MARINO", Caption = "San Marino", NumericCode = 674},
                    new Country{ ShortCode = "SN", LongCode = "SEN", Name = "SENEGAL", Caption = "Senegal", NumericCode = 686},
                    new Country{ ShortCode = "SO", LongCode = "SOM", Name = "SOMALIA", Caption = "Somalia", NumericCode = 706},
                    new Country{ ShortCode = "SR", LongCode = "SUR", Name = "SURINAME", Caption = "Suriname", NumericCode = 740},
                    new Country{ ShortCode = "ST", LongCode = "STP", Name = "SAO TOME AND PRINCIPE", Caption = "Sao Tome and Principe", NumericCode = 678},
                    new Country{ ShortCode = "SV", LongCode = "SLV", Name = "EL SALVADOR", Caption = "El Salvador", NumericCode = 222},
                    new Country{ ShortCode = "SY", LongCode = "SYR", Name = "SYRIAN ARAB REPUBLIC", Caption = "Syrian Arab Republic", NumericCode = 760},
                    new Country{ ShortCode = "SZ", LongCode = "SWZ", Name = "SWAZILAND", Caption = "Swaziland", NumericCode = 748},
                    new Country{ ShortCode = "TC", LongCode = "TCA", Name = "TURKS AND CAICOS ISLANDS", Caption = "Turks and Caicos Islands", NumericCode = 796},
                    new Country{ ShortCode = "TD", LongCode = "TCD", Name = "CHAD", Caption = "Chad", NumericCode = 148},
                    new Country{ ShortCode = "TF", LongCode = "", Name = "FRENCH SOUTHERN TERRITORIES", Caption = "French Southern Territories", NumericCode = 0},
                    new Country{ ShortCode = "TG", LongCode = "TGO", Name = "TOGO", Caption = "Togo", NumericCode = 768},
                    new Country{ ShortCode = "TH", LongCode = "THA", Name = "THAILAND", Caption = "Thailand", NumericCode = 764},
                    new Country{ ShortCode = "TJ", LongCode = "TJK", Name = "TAJIKISTAN", Caption = "Tajikistan", NumericCode = 762},
                    new Country{ ShortCode = "TK", LongCode = "TKL", Name = "TOKELAU", Caption = "Tokelau", NumericCode = 772},
                    new Country{ ShortCode = "TL", LongCode = "", Name = "TIMOR-LESTE", Caption = "Timor-Leste", NumericCode = 0},
                    new Country{ ShortCode = "TM", LongCode = "TKM", Name = "TURKMENISTAN", Caption = "Turkmenistan", NumericCode = 795},
                    new Country{ ShortCode = "TN", LongCode = "TUN", Name = "TUNISIA", Caption = "Tunisia", NumericCode = 788},
                    new Country{ ShortCode = "TO", LongCode = "TON", Name = "TONGA", Caption = "Tonga", NumericCode = 776},
                    new Country{ ShortCode = "TR", LongCode = "TUR", Name = "TURKEY", Caption = "Turkey", NumericCode = 792},
                    new Country{ ShortCode = "TT", LongCode = "TTO", Name = "TRINIDAD AND TOBAGO", Caption = "Trinidad and Tobago", NumericCode = 780},
                    new Country{ ShortCode = "TV", LongCode = "TUV", Name = "TUVALU", Caption = "Tuvalu", NumericCode = 798},
                    new Country{ ShortCode = "TW", LongCode = "TWN", Name = "TAIWAN, PROVINCE OF CHINA", Caption = "Taiwan, Province of China", NumericCode = 158},
                    new Country{ ShortCode = "TZ", LongCode = "TZA", Name = "TANZANIA, UNITED REPUBLIC OF", Caption = "Tanzania, United Republic of", NumericCode = 834},
                    new Country{ ShortCode = "UA", LongCode = "UKR", Name = "UKRAINE", Caption = "Ukraine", NumericCode = 804},
                    new Country{ ShortCode = "UG", LongCode = "UGA", Name = "UGANDA", Caption = "Uganda", NumericCode = 800},
                    new Country{ ShortCode = "UM", LongCode = "", Name = "UNITED STATES MINOR OUTLYING ISLANDS", Caption = "United States Minor Outlying Islands", NumericCode = 0},
                    new Country{ ShortCode = "US", LongCode = "USA", Name = "UNITED STATES", Caption = "United States", NumericCode = 840},
                    new Country{ ShortCode = "UY", LongCode = "URY", Name = "URUGUAY", Caption = "Uruguay", NumericCode = 858},
                    new Country{ ShortCode = "UZ", LongCode = "UZB", Name = "UZBEKISTAN", Caption = "Uzbekistan", NumericCode = 860},
                    new Country{ ShortCode = "VA", LongCode = "VAT", Name = "HOLY SEE (VATICAN CITY STATE)", Caption = "Holy See (Vatican City State)", NumericCode = 336},
                    new Country{ ShortCode = "VC", LongCode = "VCT", Name = "SAINT VINCENT AND THE GRENADINES", Caption = "Saint Vincent and the Grenadines", NumericCode = 670},
                    new Country{ ShortCode = "VE", LongCode = "VEN", Name = "VENEZUELA", Caption = "Venezuela", NumericCode = 862},
                    new Country{ ShortCode = "VG", LongCode = "VGB", Name = "VIRGIN ISLANDS, BRITISH", Caption = "Virgin Islands, British", NumericCode = 92},
                    new Country{ ShortCode = "VI", LongCode = "VIR", Name = "VIRGIN ISLANDS, U.S.", Caption = "Virgin Islands, U.s.", NumericCode = 850},
                    new Country{ ShortCode = "VN", LongCode = "VNM", Name = "VIET NAM", Caption = "Viet Nam", NumericCode = 704},
                    new Country{ ShortCode = "VU", LongCode = "VUT", Name = "VANUATU", Caption = "Vanuatu", NumericCode = 548},
                    new Country{ ShortCode = "WF", LongCode = "WLF", Name = "WALLIS AND FUTUNA", Caption = "Wallis and Futuna", NumericCode = 876},
                    new Country{ ShortCode = "WS", LongCode = "WSM", Name = "SAMOA", Caption = "Samoa", NumericCode = 882},
                    new Country{ ShortCode = "YE", LongCode = "YEM", Name = "YEMEN", Caption = "Yemen", NumericCode = 887},
                    new Country{ ShortCode = "YT", LongCode = "", Name = "MAYOTTE", Caption = "Mayotte", NumericCode = 0},
                    new Country{ ShortCode = "ZA", LongCode = "ZAF", Name = "SOUTH AFRICA", Caption = "South Africa", NumericCode = 710},
                    new Country{ ShortCode = "ZM", LongCode = "ZMB", Name = "ZAMBIA", Caption = "Zambia", NumericCode = 894},
                    new Country{ ShortCode = "ZW", LongCode = "ZWE", Name = "ZIMBABWE", Caption = "Zimbabwe", NumericCode = 716},
                };
        }
    }
}
