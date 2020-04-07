using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx.Async;
using System;

public enum Type
{
    //残骸
    //UIの種類を確立しといて、インスペクターで設定してどういう動作するか決めようと思った
    //必要なかった
    Button,
    Window,
    Bar,
}


public class UIController : MonoBehaviour
{

    //UIのシステム的な操作、及びアニメーションが発火条件を満たした時アニメーションを呼ぶスクリプト
    //アニメーション用と分けてある理由は、仮に合併すると下に並んでいるキャッシュ連中をUI全員が行わなくてはならなくなるため
    //なおこのスクリプトはUniRxを使ってる

    UIAnimator anim;
    private AsyncOperation async;
    

    [SerializeField] Image sceneBall;
    [SerializeField] Camera cameraObj;
    [SerializeField] GameObject dest;
    [SerializeField] AudioSource audioFile;
    [SerializeField] Slider slide;

    [SerializeField] CanvasGroup mainCanvas;
    [SerializeField] CanvasGroup optionCanvas;
    [SerializeField] CanvasGroup exitCanvas;
    [SerializeField] CanvasGroup videoCanvas;


    void Start()
    {
        //アニメーション用スクリプト
        anim = GetComponent<UIAnimator>();

        //フェードアウトしてシーンを切り替えてきた際、フェードインを完了するための処理
        //今回は一方通行なのでこういう書き方になっているが、本来なら互換性が要る
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Debug.Log("イエイ");
            StartCoroutine(anim.SceneChangeAni(sceneBall, 0));
        }
    }

    //シーン変えるマン
    //ボタンが押された瞬間に非同期でシーンの読み込みを開始し、フェードアウト処理も始める
    //シーンチェンジのアニメーションをawaitする事で、アニメーションが完了してからシーンを移動できる
    public async void SceneChange(int sceneNum)
    {
        async = SceneManager.LoadSceneAsync(sceneNum, LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        await anim.SceneChangeAni(sceneBall, sceneNum);

        SceneManager.LoadScene(sceneNum);
    }

    //UIを出したりしまったりする系スクリプト
    //引数としてボタンからboolをもらう事で、出すのかしまうのか判断する
    //引数Trueなら出す、Falseならしまう
    //なお、処理上bool変数と1か0のint変数の2つが必要だったので、boolだけもらってintに変換している
    //ちなみにUnityがこれを使うには最初にSystemをusingする必要がある

    //使い方(例：最初の画面からOptionを押した時)
    //入る時　Optionボタンが押された時、OptionUIをTrueで呼び、MainUIをFalseで呼ぶ
    //戻る時　Optionキャンバス内にあるBackボタンが押された時、OptionUIをFalseで呼び、MainUIをTrueで呼ぶ
    public void MainUI(bool value)
    {
        int valueint = Convert.ToInt32(value);
        anim.FadeInOut(valueint, 0.5f, DG.Tweening.Ease.OutQuart, mainCanvas);
        mainCanvas.interactable = value;
        mainCanvas.blocksRaycasts = value;
    }
    public void OptionUI(bool value)
    {
        int valueint = Convert.ToInt32(value);
        anim.FadeInOut(valueint, 0.5f, DG.Tweening.Ease.OutQuart, optionCanvas);
        optionCanvas.interactable = value;
        optionCanvas.blocksRaycasts = value;
    }
    public void ExitUI(bool value)
    {
        int valueint = Convert.ToInt32(value);
        anim.FadeInOut(valueint, 0.5f, DG.Tweening.Ease.OutQuart, exitCanvas);
        exitCanvas.interactable = value;
        exitCanvas.blocksRaycasts = value;
    }
    public void VideoUI(bool value)
    {
        int valueint = Convert.ToInt32(value);
        anim.FadeInOut(valueint, 0.5f, DG.Tweening.Ease.OutQuart, videoCanvas);
        videoCanvas.interactable = value;
        videoCanvas.blocksRaycasts = value;
    }
    //ここまで出したりしまったりする系
    //似たような処理なのに分けてるのは、ボタンは引数を1つしか持てないのでboolは渡せてもCanvasGroupを渡せないから
    //解決策になる何か良い呼び出し方あったら求む


    //ExitからYesが押された時にデスクトップ画面を出す
    //実際にはゲームをたたむ処理になる
    public void ExitWow()
    {
        dest.SetActive(true);
    }

    //ビデオにある背景色チェンジ用の処理
    //ボタンの色を取得し、それを背景に適用する
    //なお、引数にはImage本体ではなくToggleをもらっているので、GetComponentにInChildrenをつける必要がある
    //なんで最初からImageもらってないかは謎　多分アタッチするのが面倒くさかった
    public void VideoButton(Toggle toggle)
    {
        cameraObj.backgroundColor = toggle.GetComponentInChildren<Image>().color;
    }


    //曲のボリューム変える
    //スライダーのValue＝曲のVolume
    public void AudioVolume()
    {
        audioFile.volume = slide.value;
    }

}
