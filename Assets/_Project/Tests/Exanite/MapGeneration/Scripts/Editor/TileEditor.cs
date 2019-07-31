using Exanite.Core.Editor.Helpers;
using Exanite.Core.Extensions;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Exanite.MapGeneration.Editor
{
    [CustomEditor(typeof(Tile))]
    public class TileEditor : OdinEditor
    {
        private readonly Color ConnectedColor = Color.green;
        private readonly Color DisconnectedColor = Color.white;

        private Tile TargetTile { get; set; }
        private InspectorProperty ConnectionRulesProperty { get; set; }

        protected override void OnEnable()
        {
            TargetTile = target as Tile;
            ConnectionRulesProperty = Tree.GetPropertyAtPath("ConnectionRules");
        }

        protected virtual void OnSceneGUI()
        {
            Tree.UpdateTree();

            HandlesHelpers.DrawRectangle(TargetTile.transform.position, TargetTile.transform.rotation, TargetTile.transform.lossyScale);

            for (int s = 0; s < Tile.Sides; s++)
            {
                var baseRotation = Quaternion.Euler(Vector3.up * 90 * s);
                var basePosition = baseRotation * Vector3.forward * 0.5f;
                var tangent = Vector3.Cross(Vector3.up, basePosition.normalized);

                for (int c = 0; c < Tile.ConnectionsPerSide; c++)
                {
                    var position = basePosition + GetOffset(Tile.ConnectionsPerSide, c) * tangent;
                    position = TargetTile.transform.TransformPoint(position);

                    var rotation = TargetTile.transform.TransformRotation(baseRotation);

                    Handles.color = TargetTile.GetConnection((TileSide)s, c) ? ConnectedColor : DisconnectedColor;

                    Handles.Label(position, $"{(s, c)}");
                    bool clicked = Handles.Button(position, rotation, 0.5f, 0.5f, Handles.ArrowHandleCap);

                    if (clicked)
                    {
                        bool current = TargetTile.GetConnection((TileSide)s, c);
                        TargetTile.SetConnection(!current, (TileSide)s, c);

                        ConnectionRulesProperty.ValueEntry.WeakSmartValue = TargetTile.Connections;
                    }
                }
            }

            Tree.ApplyChanges();
        }

        protected float GetOffset(int connections, int index)
        {
            return ((1 + 2f * index) / (2f * connections)) - 0.5f;
        }
    }
}
