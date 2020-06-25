# VMCProtocolMultiplexer
[VMCProtocol](https://sh-akira.github.io/VirtualMotionCaptureProtocol/)を分配するソフトウェア。  
複数の入出力を持ち、配送先を自由に設定することができる。

なお現状、VMCProtocolに基づいた処理は特にしていないので、単にOSC信号を中継・分配するのに使えます。  
**本ソフトウェアは、自分で問題を解決できる人向け(上級者向け)です。**  

**[ダウンロード](https://github.com/gpsnmeajp/VMCProtocolMultiplexer/releases)**

[VMC Protocol対応](https://sh-akira.github.io/VirtualMotionCaptureProtocol/)  
<img src="https://github.com/gpsnmeajp/VMCProtocolMultiplexer/blob/master/README-image/vmpc_logo_128x128.png?raw=true"></img>

## 現在利用可能な機能
- 入力-分配-出力
- フィルタ機能は未実装

## 設定
- input.jsonで入力名、入力ポートを定義します。
- output.jsonで出力名、出力先IPアドレス、出力ポートを定義します。
- filter.jsonで入力名と出力名の対応関係を定義します。
- 起動すると中継を開始します。動作中はパケット数が表示されます。

<img src="https://github.com/gpsnmeajp/VMCProtocolMultiplexer/blob/master/README-image/image.png?raw=true"></img>

# [お問合せ先(Discordサーバー)](https://discord.gg/nGapSR7)
