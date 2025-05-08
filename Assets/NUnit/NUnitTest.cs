using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NUnitTest
{
    [Test]
    public void AddItemToInventory_WhenItemIsPickedUp()
    {
        // Создание и инициализация базы данных
        InventoryDatabase inventoryDatabase = ScriptableObject.CreateInstance<InventoryDatabase>();
        inventoryDatabase.InventoryItemConfigs = new List<InventoryItemConfig>();

        // Создание и добавление предмета
        MedicationsItemConfig item = ScriptableObject.CreateInstance<MedicationsItemConfig>();
        item.SetupId("Таблетки_7da488f2-abd0-47b2-8887-167ea790bcf9");
        inventoryDatabase.InventoryItemConfigs.Add(item);

        // Поиск предмета
        var itemadd = inventoryDatabase.FindItemByID("Таблетки_7da488f2-abd0-47b2-8887-167ea790bcf9");

        // Вывод всех предметов в InventoryItemConfigs
        Debug.Log("Все предметы в InventoryItemConfigs:");
        
        foreach (var i in inventoryDatabase.InventoryItemConfigs)
        {
            Debug.Log($"- {i.ItemID}");
        }

        // Проверка
        Assert.Contains(itemadd, inventoryDatabase.InventoryItemConfigs);
    }
}