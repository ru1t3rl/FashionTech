using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeGenerator))]
public class ShapeEditor : Editor
{
    ShapeGenerator shapeGenerator;
    SelectionInfo selectionInfo;
    bool needsRepaint;
    void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        else if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else {
            HandleInput(guiEvent);
            if (needsRepaint)
            {
                HandleUtility.Repaint();
            }
        }

    }

    void HandleInput(Event guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float distanceToDrawPlane = (drawPlaneHeight - mouseRay.origin.z) / mouseRay.direction.z;
        Vector3 mousePosition = mouseRay.GetPoint(distanceToDrawPlane);

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseDown(mousePosition);
        }

        if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseUp(mousePosition);
        }

        if (guiEvent.type == EventType.MouseDrag && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseDrag(mousePosition);
        }

        if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        if (!selectionInfo.pointIsSelected)
        {
            updateMouseOverInfo(mousePosition);
        }
    }

    void HandleLeftMouseDown(Vector3 mousePosition)
    {
        
            //adding a new point 
        if (!selectionInfo.mouseIsOverPoint)
        {
            for (int i = 0; i < 2; i++)
            {
                int newPointIndex = (selectionInfo.mouseIsOverLine) ? selectionInfo.lineIndex + 1+i : shapeGenerator.vertices.Count;
                Undo.RecordObject(shapeGenerator, "Add point");
                var vertex = shapeGenerator.CreateVertex();
                vertex.point = mousePosition;
                shapeGenerator.vertices.Insert(newPointIndex, vertex);
                selectionInfo.pointIndex = newPointIndex;
             
            }
            SetPointAndNormals(selectionInfo.pointIndex, mousePosition);
            SetLineIndices();

        }

        selectionInfo.pointIsSelected = true;
        selectionInfo.positionAtStartOfDrag = mousePosition;
        needsRepaint = true;
    }

    void HandleLeftMouseUp(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            SetPointAndNormals(selectionInfo.pointIndex, selectionInfo.positionAtStartOfDrag);
            Undo.RecordObject(shapeGenerator, "move point");
            SetPointAndNormals(selectionInfo.pointIndex, mousePosition);

            selectionInfo.pointIsSelected = false;
            selectionInfo.pointIndex = -1;
            needsRepaint = true;
        }
    }

    void HandleLeftMouseDrag(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            SetPointAndNormals(selectionInfo.pointIndex, mousePosition);
            needsRepaint = true;
        }
    }

    void SetPointAndNormals(int index, Vector3 position)
    {
        shapeGenerator.vertices[index - 1].point = position;
        shapeGenerator.vertices[index].point = position;

        if (index - 2 > 0)
        {
            float dx = shapeGenerator.vertices[index - 1].point.x - shapeGenerator.vertices[index - 2].point.x;
            float dy = shapeGenerator.vertices[index - 1].point.y - shapeGenerator.vertices[index - 2].point.y;
            Vector2 normal = new Vector2(-dy, dx);
            shapeGenerator.vertices[index - 1].normal = normal;
            shapeGenerator.vertices[index - 2].normal = normal;

        }
        if (index != shapeGenerator.vertices.Count - 1)
        {
            float dx = shapeGenerator.vertices[index + 1].point.x - shapeGenerator.vertices[index].point.x;
            float dy = shapeGenerator.vertices[index + 1].point.y - shapeGenerator.vertices[index].point.y;
            Vector2 normal = new Vector2(-dy, dx);
            shapeGenerator.vertices[index + 1].normal = normal;
            shapeGenerator.vertices[index].normal = normal;
        }
        else
        if (shapeGenerator.closeShape && index == shapeGenerator.vertices.Count - 1)
        {

            float dx = shapeGenerator.vertices[0].point.x - shapeGenerator.vertices[index].point.x;
            float dy = shapeGenerator.vertices[0].point.y - shapeGenerator.vertices[index].point.y;
            Vector2 normal = new Vector2(-dy, dx);
            shapeGenerator.vertices[0].normal = normal;
            shapeGenerator.vertices[index].normal = normal;
        }

        SetUData();
        
 
        
    }

    public void SetLineIndices()
    {
        shapeGenerator.lineIndices.Clear();
        for (int i = 0; i < shapeGenerator.vertices.Count; i++)
        {
            shapeGenerator.lineIndices.Add(i == 0 ? shapeGenerator.vertices.Count - 1 : i - 1);
        }

    }
    public void  SetUData()
    {
        float totalDistance = 0;
        for (int i = 0; i < shapeGenerator.vertices.Count; i++)
        {
            shapeGenerator.vertices[i].u = totalDistance;
            if (i == shapeGenerator.vertices.Count - 1)
            {
                totalDistance += Vector2.Distance(shapeGenerator.vertices[i].point, shapeGenerator.vertices[0].point);
            }
            else
            {
                totalDistance += Vector2.Distance(shapeGenerator.vertices[i].point, shapeGenerator.vertices[i + 1].point);
            }
        }

        for (int j = 0; j < shapeGenerator.vertices.Count; j++)
        {
            shapeGenerator.vertices[j].u = j != 0 ? shapeGenerator.vertices[j].u/ totalDistance : 1;
            Debug.Log(shapeGenerator.vertices[j].u);
        }
    }

    void Draw()
    {
        for (int i = 0; i < shapeGenerator.vertices.Count; i++)
        {
            Vector3 nextPoint = ((i + 1) % shapeGenerator.vertices.Count) == 1 && !shapeGenerator.closeShape?  shapeGenerator.vertices[(i)].point : shapeGenerator.vertices[(i + 1) % shapeGenerator.vertices.Count].point ;
            if (i == selectionInfo.lineIndex)
            {
                Handles.color = Color.red;
                Handles.DrawLine(shapeGenerator.vertices[i].point, nextPoint);
            }
            else
            {
                Handles.color = Color.black;
                Handles.DrawDottedLine(new Vector3(shapeGenerator.vertices[i].point.x, shapeGenerator.vertices[i].point.y,0), nextPoint, 4);
            }

            if (i == selectionInfo.pointIndex)
            {
                Handles.color = (selectionInfo.pointIsSelected ? Color.blue : Color.yellow);
            }
            else
            {
                Handles.color = Color.white;
            }
            Handles.DrawSolidDisc(shapeGenerator.vertices[i].point, Vector3.forward, shapeGenerator.handleRadius);
        }
        needsRepaint = false;
    }

    void updateMouseOverInfo(Vector3 mousePosition)
    {
        int mouseOverPointIndex = -1;
        for (int i = 1; i < shapeGenerator.vertices.Count; i+=2)
        {
            if (Vector3.Distance(mousePosition, shapeGenerator.vertices[i].point) < shapeGenerator.handleRadius)
            {
                mouseOverPointIndex = i;
                break;
            }
        }

        if(mouseOverPointIndex != selectionInfo.pointIndex)
        {
            selectionInfo.pointIndex = mouseOverPointIndex;
            selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

            needsRepaint = true;
        }

        if (selectionInfo.mouseIsOverPoint)
        {
            selectionInfo.mouseIsOverLine = false;
            selectionInfo.lineIndex = -1;
        }
        else
        {
            int mouseOverLineIndex = -1;
            float closestLineDistance = shapeGenerator.handleRadius;
            for (int i = 0; i < shapeGenerator.vertices.Count; i++)
            {
                Vector3 nextPointInShape = shapeGenerator.vertices[(i + 1)%shapeGenerator.vertices.Count].point;
                float distanceFromMouseToLine = HandleUtility.DistancePointToLineSegment(mousePosition.ToXY(), shapeGenerator.vertices[i].point, nextPointInShape);
                if (distanceFromMouseToLine < shapeGenerator.handleRadius)
                {
                    closestLineDistance = distanceFromMouseToLine;
                    mouseOverLineIndex = i;
                }
            }

            if(selectionInfo.lineIndex != mouseOverLineIndex)
            {
                selectionInfo.lineIndex = mouseOverLineIndex;
                selectionInfo.mouseIsOverLine = mouseOverLineIndex != -1;
                needsRepaint = true;
            }
        }
    }

    void OnEnable()
    {
        shapeGenerator = target as ShapeGenerator;
        selectionInfo = new SelectionInfo();
    }

    public class SelectionInfo {
        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
        public Vector3 positionAtStartOfDrag;

        public int lineIndex = -1;
        public bool mouseIsOverLine;
    }
}
