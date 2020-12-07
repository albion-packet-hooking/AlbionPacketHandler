using AlbionMarshaller.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionMarshaller.Model
{
    public class Item
    {
        private readonly string LocalizationItemPrefix = "@ITEMS_";
        private readonly string LocalizationItemDescPostfix = "_DESC";

        public int Id { get; set; }

        public String DescriptiveName { get; set; }

        public String UniqueName { get; set; }
        public String EnchantmentLevel { get; set; }
        public String Description { get; set; }
        public String Subcategory { get; set; }

        private string _cachedItemName = null;
        public String ItemName
        {
            get
            {
                if(_cachedItemName == null && UniqueName != null)
                {
                    if(UniqueName.Contains("@"))
                    {
                        _cachedItemName = UniqueName.Split('@')[0];
                    }
                    else
                    {
                        _cachedItemName = UniqueName;
                    }
                }

                return _cachedItemName;
            }
        }

        public String LocalizationName
        {
            get
            {
                if (DescriptiveName != null)
                {
                    return DescriptiveName;
                }
                else
                {
                    return LocalizationItemPrefix + ItemName;
                }
            }
        }

        private string _cachedName = null;
        public String LocalizedName
        {
            get
            {
                if (_cachedName == null)
                {
                    _cachedName = Localization.Instance.Find(LocalizationName);
                }

                return _cachedName;
            }
        }

        private string _cachedDescription = null;
        public String LocalizedDescription
        {
            get
            {
                if(_cachedDescription == null)
                {
                    if (Description != null)
                    {
                        _cachedDescription = Localization.Instance.Find(Description);
                    }
                    else
                    {
                        _cachedDescription = Localization.Instance.Find(LocalizationItemPrefix + ItemName + LocalizationItemDescPostfix);
                    }
                }

                return _cachedDescription;
            }
        }
    }
}
