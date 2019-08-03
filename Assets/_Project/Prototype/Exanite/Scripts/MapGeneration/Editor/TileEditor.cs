using Exanite.Core.Editor.Helpers;
using Exanite.Core.Extensions;
using Exanite.MapGeneration.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exanite.MapGeneration.Editor
{
    [CustomEditor(typeof(Tile))]
    public class TileEditor : UnityEditor.Editor
    {
        private readonly Color ConnectedColor = Color.green;
        private readonly Color DisconnectedColor = Color.white;

        private Tile TargetTile { get; set; }

        private void OnEnable()
        {
            TargetTile = target as Tile;
        }

        protected virtual void OnSceneGUI()
        {
            HandlesHelpers.DrawRectangle(TargetTile.transform.position, TargetTile.transform.rotation, TargetTile.transform.lossyScale);

            for (int x = 0; x < Tile.Sides; x++) // draws arrows starting from PositiveZ side going clockwise
            {
                var baseRotation = Quaternion.Euler(Vector3.up * 90 * x);
                var basePosition = baseRotation * Vector3.forward * 0.5f;
                var tangent = Vector3.Cross(Vector3.up, basePosition.normalized);

                for (int y = 0; y < Tile.ConnectionsPerSide; y++)
                {
                    // rotate by the inverse of the target tile's rotation to cancel out TransformRotation's y-axis rotation
                    // then flip by the target tile's flip axis to cancel out TransformPoint applying its own scale
                    var side = (TileSide)x;
                    side = side.Rotate(TargetTile.Rotation.Inverse());
                    side = side.Flip(TargetTile.Flip);

                    // if only axis is flipped (none or both flips cancel out), reverse connections
                    bool onlyOneAxisFlipped = TargetTile.Flip.HasFlag(TileFlip.FlipX) != TargetTile.Flip.HasFlag(TileFlip.FlipZ);
                    int connection = onlyOneAxisFlipped ? (int)MathHelper.Remap(y, 0f, 2f, 2f, 0f) : y;

                    var position = basePosition + GetOffset(Tile.ConnectionsPerSide, y) * tangent;
                    position = TargetTile.transform.TransformPoint(position);

                    var rotation = TargetTile.transform.TransformRotation(baseRotation);

                    Handles.color = TargetTile.GetConnection(side, connection) ? ConnectedColor : DisconnectedColor;

                    bool clicked = Handles.Button(position, rotation, 0.5f, 0.5f, Handles.ArrowHandleCap);

                    if (clicked)
                    {
                        bool current = TargetTile.GetConnection(side, connection);
                        TargetTile.SetConnection(!current, side, connection);

                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    }
                }
            }
        }

        protected float GetOffset(int connections, int index)
        {
            return ((1 + 2f * index) / (2f * connections)) - 0.5f;
        }
    }
}
