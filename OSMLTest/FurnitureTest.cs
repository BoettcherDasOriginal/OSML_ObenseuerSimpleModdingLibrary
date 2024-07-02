using System;
using System.Collections.Generic;
using System.Text;
using OSML;
using UnityEngine;

namespace OSMLTest
{
    public class FurnitureTest : MonoBehaviour
    {
        private bool _trigger = true;

        private void Update()
        {
            if(PublicVars.instance.firstUpdateFinished && _trigger)
            {
                _trigger = false;

                GameObject prefabStuff = new GameObject("stuff");
                prefabStuff.AddComponent<MeshFilter>();
                prefabStuff.AddComponent<MeshRenderer>();

                GameObject prePrefabStuff = new GameObject("stuff");
                prePrefabStuff.AddComponent<MeshFilter>();
                prePrefabStuff.AddComponent<MeshRenderer>();

                Mesh mesh = new Mesh();

                Vector3[] vertices = new[]
                {
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 0),
                };
                mesh.vertices = vertices;

                Vector2[] uv = new[]
                {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1),
                };
                mesh.uv = uv;

                Vector3[] normals = new[]
                {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                };
                mesh.normals = normals;

                int[] triangles = new[]
                {
                0,1,2,
                2,1,3
                };
                mesh.triangles = triangles;

                Material mat = new Material(Shader.Find("Standard"));

                prefabStuff.GetComponent<MeshFilter>().mesh = mesh;
                prefabStuff.GetComponent<MeshRenderer>().material = mat;
                prePrefabStuff.GetComponent<MeshFilter>().mesh = mesh;
                prePrefabStuff.GetComponent<MeshRenderer>().material = mat;

                prefabStuff.AddComponent<BoxCollider>();
                var bc = prePrefabStuff.AddComponent<BoxCollider>();
                bc.isTrigger = true;
                var rb = prePrefabStuff.AddComponent<Rigidbody>();
                rb.isKinematic = true;

                Furniture f = FurnitureCreator.NewFurniture("SuperDupperTestDeco",
                    Sprite.Create(new Texture2D(64, 64), new Rect(Vector2.zero, new Vector2(64, 64)), Vector2.zero),
                    "Test,1,2,3...",
                    Furniture.Category.Decoration,
                    0,
                    0,
                    prefabStuff,
                    prePrefabStuff,
                    new Furniture.BuildingArea[] { },
                    new System.Collections.Generic.List<Furniture.ReseourceItem>()
                );

                f.GiveItem();
            }
        }
    }
}
