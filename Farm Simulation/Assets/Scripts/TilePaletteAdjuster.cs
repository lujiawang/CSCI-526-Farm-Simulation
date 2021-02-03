/*This script make tile palette more seperate. */
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace z
{
    internal sealed class TilePaletteAdjuster : EditorWindow
    {
        [SerializeField]
        private int Extent = 10;

        [SerializeField]
        private GameObject Prefab;

        [SerializeField]
        private bool Square = true;

        [SerializeField]
        private int Stride = 3;

        private Tilemap Tilemap => Prefab != null ? Prefab.GetComponentInChildren<Tilemap>() : null;

        [MenuItem("z/Tile Palette Adjuster")]
        private static void ShowWindow()
        {
            var window = GetWindow<TilePaletteAdjuster>(true, "Tile Palette Adjuster", true);
            window.minSize = window.maxSize = new Vector2(333.0f, 111.0f);
        }

        private void OnGUI()
        {
            Prefab = EditorGUILayout.ObjectField(new GUIContent("Prefab", "The tile palette to adjust."),
                Prefab, typeof(GameObject), false) as GameObject;

            using (new EditorGUI.DisabledGroupScope(Square))
            {
                Extent = EditorGUILayout.IntSlider(
                    new GUIContent("Extent", "The extend to distribute tiles on."), Extent, 1, 20);
            }

            Stride = EditorGUILayout.IntSlider(
                new GUIContent("Stride", "The stride between each tiles."), Stride, 1, 10);

            Square = EditorGUILayout.Toggle(
                new GUIContent("Square", "Distribute tiles in a square fashion."), Square);

            EditorGUILayout.Space();

            var disabled = Prefab == null ||
                           PrefabUtility.GetPrefabAssetType(Prefab) != PrefabAssetType.Regular ||
                           Tilemap == null;

            using (new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.DisabledGroupScope(disabled))
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Adjust", GUILayout.Width(75.0f)))
                    Adjust();
            }
        }

        private void Adjust()
        {
            var tilemap = Tilemap;
            if (tilemap is null)
                throw new ArgumentNullException(nameof(tilemap));

            var block = tilemap.GetTilesBlock(tilemap.cellBounds);
            var tiles = block.Where(s => s != null).OrderBy(s => s.name).ToArray();
            var ceil = (int)Mathf.Ceil(Mathf.Sqrt(tiles.Length));
            var w = Square ? ceil : Extent;
            var h = Square ? ceil : tiles.Length / Extent + (tiles.Length % Extent == 0 ? 0 : 1);
            var z = tilemap.cellBounds.size.z;

            var array = new TileBase[w * Stride * h * Stride];
            var index = 0;

            foreach (var tile in tiles)
            {
                var x = index % w * Stride;
                var y = index / w * Stride;
                var i = y * w * Stride + x;
                array[i] = tile;
                index++;
            }

            // if we save asset we lose undo, but then preview in Tile Palette fails, this hack allows both
            Selection.activeObject = Prefab;

            Undo.RecordObject(tilemap, "Adjust Tile Palette");

            tilemap.ClearAllTiles();
            tilemap.size = new Vector3Int(w * Stride, h * Stride, z);
            tilemap.SetTilesBlock(tilemap.cellBounds, array);

            PrefabUtility.RecordPrefabInstancePropertyModifications(tilemap);
            EditorUtility.SetDirty(Prefab);
        }
    }
}