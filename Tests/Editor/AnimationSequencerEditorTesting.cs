using System;
using System.Collections;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using UnityEngine.UI;

public static class AssertUtility
{
    public static void AreTransformAtPosition(Transform transform, Vector3 position, string message)
    {
        AreApproximatelyEqual(transform.position, position, message);
    }
    
    public static void AreApproximatelyEqual(Vector3 valueA, Vector3 valueB, string message)
    {
        Assert.AreApproximatelyEqual(valueA.x, valueB.x, message);
        Assert.AreApproximatelyEqual(valueA.y, valueB.y, message);
        Assert.AreApproximatelyEqual(valueA.z, valueB.z, message);
    }

    public static void AreRectTransformAnchoredPositionAtPosition(RectTransform rectTransform, Vector2 position, string message)
    {
        AreApproximatelyEqual(rectTransform.anchoredPosition, position, message);
    }
    
    public static void AreApproximatelyEqual(Vector2 valueA, Vector2 valueB, string message)
    {
        Assert.AreApproximatelyEqual(valueA.x, valueB.x, message);
        Assert.AreApproximatelyEqual(valueA.y, valueB.y, message);
    }
}
public class Wait
{
    public static IEnumerator Until(Func<bool> condition, float timeout = 30f)
    {
        float timePassed = 0f;
        while (!condition() && timePassed < timeout) {
            yield return null;
            timePassed += Time.deltaTime;
        }
        if (timePassed >= timeout) {
            throw new TimeoutException("Condition was not fulfilled for " + timeout + " seconds.");
        }
    }

    public static IEnumerator ForSeconds(float duration)
    {
        float timePassed = 0f;
        while (timePassed < duration) {
            yield return null;
            timePassed += Time.deltaTime;
        }
    }
}
public class AnimationSequencerEditorTesting
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator AnimationSequencerEditorTestingWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator ScaleTween_Test()
    {
        GameObject newGameObject = new GameObject("Sequencer Test");
        AnimationSequencerController animationSequencerController =
            newGameObject.AddComponent<AnimationSequencerController>();

        Assert.IsNotNull(animationSequencerController, "Sequencer Controller failed to be added");
        
        
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(0, 0, 0);


        DOTweenAnimationStep dotweenAnimationStep = new DOTweenAnimationStep(sphere, 5);
        animationSequencerController.AddAnimationStep(dotweenAnimationStep);

        Assert.AreEqual(animationSequencerController.AnimationSteps.Length, 1, "Animation steps Length should be 1");

        ScaleDOTweenAction scaleAction =
            new ScaleDOTweenAction(Vector3.zero, AxisConstraint.X | AxisConstraint.Y | AxisConstraint.Z);
        
        dotweenAnimationStep.AddAction(scaleAction);
        
        Assert.AreEqual(dotweenAnimationStep.Actions.Length, 1, "Animation Action steps Length should be 1");
        
        
        AnimationSequencerControllerCustomEditor animationSequencerCustomEditor =
            Editor.CreateEditor(animationSequencerController) as AnimationSequencerControllerCustomEditor;

        animationSequencerCustomEditor.PlaySequence();

        yield return Wait.Until(() => !animationSequencerController.IsPlaying);

        Assert.AreApproximatelyEqual(sphere.transform.localScale.magnitude, 0, "Scale of the sphere should be 0");
    }

    [UnityTest]
    public IEnumerator DelayIssue_Test()
    {
        RectTransform canvasRectTransform = CreateCanvas();
        GameObject imageGO = new GameObject("Image");
        imageGO.transform.SetParent(canvasRectTransform);
        Image image = imageGO.AddComponent<Image>();
        image.transform.localPosition = Vector3.zero;
        Assert.IsNotNull(image, "Image should have been created properly ");
        
        GameObject newGameObject = new GameObject("Sequencer Test");
        AnimationSequencerController animationSequencerController =
            newGameObject.AddComponent<AnimationSequencerController>();

        Assert.IsNotNull(animationSequencerController, "Sequencer Controller failed to be added");
        
        DOTweenAnimationStep dotweenAnimationStep = new DOTweenAnimationStep(image.gameObject, 1, 1);
        animationSequencerController.AddAnimationStep(dotweenAnimationStep);

        Assert.AreEqual(animationSequencerController.AnimationSteps.Length, 1, "Animation steps Length should be 1");

        AnchoredPositionMoveToPositionDOTweenActionBase anchoredFirst = new AnchoredPositionMoveToPositionDOTweenActionBase(new Vector2(200, 0), true);
        dotweenAnimationStep.AddAction(anchoredFirst);
        AnchoredPositionMoveToPositionDOTweenActionBase anchoredSecond = new AnchoredPositionMoveToPositionDOTweenActionBase(new Vector2(0, 200), true);
        dotweenAnimationStep.AddAction(anchoredSecond);
        
        AnimationSequencerControllerCustomEditor animationSequencerCustomEditor =
            Editor.CreateEditor(animationSequencerController) as AnimationSequencerControllerCustomEditor;

        animationSequencerCustomEditor.PlaySequence();

        yield return Wait.ForSeconds(1);
        Assert.AreApproximatelyEqual(image.transform.localPosition.magnitude, 0,
            "Image moved before delay was finished");
        
        yield return Wait.ForSeconds(1);
        RectTransform imageRectTransform = image.transform as RectTransform;
        AssertUtility.AreRectTransformAnchoredPositionAtPosition(imageRectTransform, new Vector2(200, 200),
            "Image should be at 200x200 now");
    }

    private RectTransform CreateCanvas()
    {
        GameObject canvas = new GameObject("Canvas");
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        return canvas.transform as RectTransform;
    }
}
