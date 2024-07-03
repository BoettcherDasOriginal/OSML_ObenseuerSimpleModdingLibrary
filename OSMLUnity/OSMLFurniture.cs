using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

namespace OSMLUnity
{
    public class OSMLFurniture : MonoBehaviour
    {
        [Header("Info")]
        public string title;

        public Sprite image;

        public string details;

        public FurnitureCategory category;

        public int priceOC;

        public int priceRM;

        [Tooltip("Allowed to put everywhere if empty")]
        public FurnitureBuildingArea[] restrictedAreas;

        // [Header("Dismantle")]
        // public List<Furniture.ReseourceItem> dismantleItems;

        [Header("Prefab")]
        public GameObject prefab;

        public GameObject previewPrefab;

        public FurnitureDisplayStyle displayStyle = FurnitureDisplayStyle.Default;

        public int displayRotationY = 0;
    }

    public enum FurnitureCategory
    {
        All,
        Chairs,
        Tables,
        Electronics,
        Paintings,
        Lights,
        Rugs,
        Items,
        Machines,
        Storage,
        Clutter,
        Beds,
        Sofas,
        Decoration,
        Plants,
        Shelves,
        Manufacturing,
        Growing,
        Letters,
        Bathroom,
        Signs,
        Magazines,
        Posters
    }

    public enum FurnitureDisplayStyle
    {
        Default,
        Painting,
        Ceiling
    }

    public enum FurnitureBuildingArea
    {
        None,
        Stairs,
        PlayerApartment,
        Workshop,
        Outside,
        Greenhouse
    }
}
