using AlbionMarshaller.Extractor;
using AlbionMarshaller.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace AlbionMarshaller.MemoryStorage
{
    public class ItemDB
    {
        private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();
        private ItemDB()
        {
            XDocument itemDoc = ResourceLoader.LoadResource("items");

            List<String> journals = new List<String>();
            int index = 0;
            foreach (XElement itemEle in itemDoc.Root.Elements())
            {
                XAttribute uniqueNameAtt = itemEle.Attribute("uniquename") ?? null;
                XAttribute enchantmentLevel = itemEle.Attribute("enchantmentlevel") ?? null;
                XAttribute description = itemEle.Attribute("descriptionlocatag") ?? null;
                XAttribute descriptionName = itemEle.Attribute("descvariable0") ?? null;
                XAttribute subCategory = itemEle.Attribute("shopsubcategory1") ?? null;

                String uniqueName = uniqueNameAtt != null ? uniqueNameAtt.Value : null;
                if (uniqueName != null && enchantmentLevel != null && enchantmentLevel.Value != "0")
                {
                    uniqueName += "@" + enchantmentLevel.Value;
                }

                itemDictionary.Add(index,
                    new Item()
                    {
                        Id = index,
                        UniqueName = uniqueName,
                        EnchantmentLevel = enchantmentLevel != null ? enchantmentLevel.Value : null,
                        Description = description != null ? description.Value : null,
                        DescriptiveName = descriptionName != null ? descriptionName.Value : null,
                        Subcategory = subCategory != null ? subCategory.Value : null
                    });

                index++;

                if (itemEle.Name == "journalitem")
                {
                    journals.Add(uniqueName);
                }

                XElement enchantments = itemEle.Element("enchantments");
                if (enchantments != null)
                {
                    IEnumerable<XElement> enchantmentLevels = enchantments.Elements("enchantment");
                    foreach (XElement enchantment in enchantmentLevels)
                    {
                        string eUniqueName = uniqueNameAtt.Value + "@" + enchantment.Attribute("enchantmentlevel").Value;
                        itemDictionary.Add(index,
                            new Item()
                            {
                                Id = index,
                                UniqueName = eUniqueName,
                                EnchantmentLevel = enchantment.Attribute("enchantmentlevel").Value,
                                Description = description != null ? description.Value : null,
                                DescriptiveName = descriptionName != null ? descriptionName.Value : null,
                                Subcategory = subCategory != null ? subCategory.Value : null
                            });

                        index++;
                    }
                }
            }

            foreach (String j in journals)
            {
                string emptyName = j + "_EMPTY";
                itemDictionary.Add(index,
                        new Item()
                        {
                            Id = index,
                            UniqueName = emptyName

                        }
                    );
                index++;
                string fullName = j + "_FULL";
                itemDictionary.Add(index,
                        new Item()
                        {
                            Id = index,
                            UniqueName = fullName

                        }
                    );
                index++;
            }
        }

        private static readonly Lazy<ItemDB> lazy = new Lazy<ItemDB>(() => new ItemDB());
        public static ItemDB Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public Item FindItem(int id)
        {
            return itemDictionary.ContainsKey(id) ? itemDictionary[id] : null;
        }
    }
}
