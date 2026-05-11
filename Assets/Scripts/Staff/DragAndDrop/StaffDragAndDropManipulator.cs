using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragAndDropManipulator : PointerManipulator
{
    // Write a constructor to set target and store a reference to the
    // root of the visual tree.
    public DragAndDropManipulator(VisualElement target)
    {
        this.target = target;
        root = target.parent;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        // Register the four callbacks on target.
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        // Un-register the four callbacks from target.
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    private Vector2 targetStartPosition { get; set; }

    private Vector3 pointerStartPosition { get; set; }

    private bool enabled { get; set; }

    private VisualElement root { get; }

    // This method stores the starting position of target and the pointer,
    // makes target capture the pointer, and denotes that a drag is now in progress.
    private void PointerDownHandler(PointerDownEvent evt)
    {
        // Use local transform position
        targetStartPosition = target.transform.position;
        pointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        enabled = true;

        // Bring to front so it doesn't go "under" other slots while dragging
        target.BringToFront();
    }

    // This method checks whether a drag is in progress and whether target has captured the pointer.
    // If both are true, calculates a new position for target within the bounds of the window.
    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            // Calculate how far the mouse has moved from the start point
            Vector3 pointerDelta = evt.position - pointerStartPosition;

            // Apply that delta directly to the starting position
            // We remove the "Clamping" logic for now to ensure it moves freely
            target.transform.position = new Vector3(
                targetStartPosition.x + pointerDelta.x,
                targetStartPosition.y + pointerDelta.y,
                0);
        }
    }

    // This method checks whether a drag is in progress and whether target has captured the pointer.
    // If both are true, makes target release the pointer.
    private void PointerUpHandler(PointerUpEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            target.ReleasePointer(evt.pointerId);
        }
    }

    // This method checks whether a drag is in progress. If true, queries the root
    // of the visual tree to find all slots, decides which slot is the closest one
    // that overlaps target, and sets the position of target so that it rests on top
    // of that slot. Sets the position of target back to its original position
    // if there is no overlapping slot.
    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (enabled)
        {
            // FIX 1: Look at the panel's visualTree (the entire window) 
            // instead of just the immediate parent.
            VisualElement rootVisualTree = target.panel.visualTree;

            UQueryBuilder<VisualElement> allSlots =
                rootVisualTree.Query<VisualElement>(className: "slot");

            VisualElement closestOverlappingSlot = null;
            float bestDistance = float.MaxValue;

            foreach (var slot in allSlots.ToList())
            {
                if (target.worldBound.Overlaps(slot.worldBound))
                {
                    float dist = Vector2.Distance(target.worldBound.center, slot.worldBound.center);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        closestOverlappingSlot = slot;
                    }
                }
            }

            // FIX 2: Handle the move or the reset
            if (closestOverlappingSlot != null)
            {
                // Only move if the slot is empty (Noita style)
                if (closestOverlappingSlot.childCount == 0)
                {
                    closestOverlappingSlot.Add(target);
                }
            }

            // FIX 3: Reset local position to zero.
            // Because we use 'Add(target)', the spell is now a child of the slot.
            // Setting position to Zero centers it perfectly in the new slot.
            target.transform.position = Vector3.zero;

            enabled = false;
        }
    }

    private bool OverlapsTarget(VisualElement slot)
    {
        return target.worldBound.Overlaps(slot.worldBound);
    }

    private VisualElement FindClosestSlot(UQueryBuilder<VisualElement> slots)
    {
        List<VisualElement> slotsList = slots.ToList();
        float bestDistanceSq = float.MaxValue;
        VisualElement closest = null;
        foreach (VisualElement slot in slotsList)
        {
            Vector3 displacement =
                RootSpaceOfSlot(slot) - target.transform.position;
            float distanceSq = displacement.sqrMagnitude;
            if (distanceSq < bestDistanceSq)
            {
                bestDistanceSq = distanceSq;
                closest = slot;
            }
        }
        return closest;
    }

    private Vector3 RootSpaceOfSlot(VisualElement slot)
    {
        Vector2 slotWorldSpace = slot.parent.LocalToWorld(slot.layout.position);
        return root.WorldToLocal(slotWorldSpace);
    }
}

