using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAnimator  : MonoBehaviour
{

    //UIのアニメーションを行うクラス。必要に応じてKantokuオブジェクトやButtonから呼ばれる

    public Ease easetype;
    float fadeTime = 0.5f;
    Ease fadeType = Ease.OutBack;


    //ボタンが押し込まれた時に縮小する
    public void ButtonPushedAni(GameObject target)
    {
        target.transform.DOScale(new Vector3(0.9f, 0.9f, 1), 0.2f);
    }

    //ボタンが離された時元のサイズに戻る
    public void ButtonUpAni(GameObject target)
    {
        target.transform.DOScale(Vector3.one, 0.3f);
    }

    //ボタンにカーソルが合った時色を白っぽくする
    public void ButtonFocused(GameObject target)
    {
        target.GetComponent<Image>().DOColor(new Color32(167, 255, 245, 255), fadeTime)
            .SetEase(fadeType);
    }

    //ボタンにカーソルが合った時色を白っぽくする(赤ボタン用)
    public void ButtonFocusedRed(GameObject target)
    {
        target.GetComponent<Image>().DOColor(new Color32(221, 151, 157, 255), fadeTime)
         .SetEase(fadeType);
    }

    //カーソルがボタンから離れた時色とサイズを元に戻す
    //サイズを戻す必要があるのは、ボタン押し込み＞そのままドラッグが行われた際フォーカルが外れた事を示すためサイズを元に戻す動作のため
    public void ButtonExit(GameObject target)
    {
        target.GetComponent<Image>().DOColor(new Color32(42, 255, 231, 255), fadeTime)
             .SetEase(fadeType);
        target.transform.DOScale(Vector3.one, 0.3f);
    }

    //カーソルがボタンから離れた時色とサイズを元に戻す(赤ボタン用)
    public void ButtonExitRed(GameObject target)
    {
        target.GetComponent<Image>().DOColor(new Color32(190, 35, 47, 255), fadeTime)
             .SetEase(fadeType);
        target.transform.DOScale(Vector3.one, 0.3f);
    }

    //ボタンにカーソルが合った時色を白っぽくする(X用)
    public void XButtonFocusedRed(GameObject target)
    {
        target.GetComponent<Image>().DOColor(new Color32(221, 151, 157, 255), fadeTime)
         .SetEase(fadeType);
    }

    //カーソルがボタンから離れた時色とサイズを元に戻す(X用)
    public void XButtonExitRed(GameObject target)
    {
        target.GetComponent<Image>().DOColor(new Color32(190, 35, 47, 255), fadeTime)
             .SetEase(fadeType);
    }


    //シーンを変えるためにフェードを引っ張ってくる
    //コルーチンで1s待機しているのは、アニメーションを完了してからロードを読み込むため
    //非同期でアニメーションを開始すると、ロード速度＞アニメーションの時間になるので、アニメーション途中でシーンが移ってしまう

    public IEnumerator SceneChangeAni(Image sceneBall, int sceneNum)
    {
        if (sceneNum == 1)
        {
            Sequence sequence = DOTween.Sequence()
                        .OnStart(() =>
                        {
                            sceneBall.transform.DOLocalMoveX(1700, 0.1f);
                        })
            .Append(sceneBall.transform.DOLocalMoveX(0, 1f));

            yield return new WaitForSeconds(1);
        }

        else if (sceneNum == 0)
        {
            Debug.Log("ウォウウォウ");
            Sequence sequence = DOTween.Sequence()
                        .OnStart(() =>
                        {
                            sceneBall.transform.DOLocalMoveX(0, 0.1f);
                        })
            .Append(sceneBall.transform.DOLocalMoveX(-1700, 1f));

            yield return new WaitForSeconds(1);
        }
    }


    //フェードインまたはアウトの時に使う汎用メソッド
    //valueintは1か0なのでこの数字でフェードインかアウトか決まる
    //durationはアニメーション時間、typeはEaseのタイプ、canvasはフェードインorアウトさせる対象
    public void FadeInOut(int valueint, float duration, Ease type, CanvasGroup canvas)
    {
        canvas.DOFade(valueint, duration)
            .SetEase(type);
    }
}
