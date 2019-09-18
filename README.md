# AudioManager

Thanks for checking out the project!

> This framework will remain `Open Source`

## Status and prerequisites

Current status at a glance:
```
Unity version: 2019.2.1f1
Platforms    : Windows/Mac/Linux/Android/IOS
```


## Features

- Easy To Use Music and SFX Managing System.
- Basic Music and SFX Mute Unmute System.
- Basic Music and SFX Volume Changing System (UI Sliders).
- Crossfading Between Music Tracks.
- Play Random Sounds(SFX) Each Time The Function Is Called.
- One Line Of Code To Play/Switch SFX/MusicTrack.
- Easy To Configure Diffrent SFX And Music Tracks.


## How it works ?
- Every Sound Or Music Track Is Stored As A Key Inside AudioManager Script.
- Everytime The Function Runs Which Contains The Code To Play The SFX Or Music, It Calls The AudioManager's Key Which We Stored In The Previous Step.


## Setting Up

- Attach The AudioManager Script To MainCamera or Any Other GameObject.
- Provide Necessary Sprites - MusicOn, MusicOff, SfxOn, SfxOff.
- Provide Music Button And Sfx Button (Just Two Normal UI Buttons).
- If You Want To Use Sliders For Changing Volumes Provide That Too.

### Adding Music
- In Inspector Of AudioManager, Under MusicTracks Tittle Click On Add.
- Give It A Preferable Name Of Your Choice Which You Can Remember In This Case "Music01".
```
NOTE :- The Name Is The Key Mentioned Above In The How It Works Section. You Have To Remember The Key As It Will Be Used In A Single Line Code To Play/Switch SFX/Music.
```
- Provide That With An Audio Clip.

### Adding SFX
- In Inspector Of AudioManager, Under Sounds Tittle Click On Add.
- Give It A Preferable Name Of Your Choice Which You Can Remember In This Case "Bounce".
```
NOTE :- The Name Is The Key Mentioned Above In The How It Works Session. You Have To Remember The Key As It Will Be Used In A Single Line Code To Play/Switch SFX/Music.
```
- Provide That With A Single Or Multiple Audio Clips (As We Can Play Audio Randomly From The Array).

![Image](https://github.com/MohitSethi99/AudioManager/blob/master/Documentation/AudioManager.PNG)

## Play Music With Code

In The Function Where You Want To Play/Switch The Music Track Add This Line Of Code.

```AudioManager.instance.PlayMusic ("KEY Of That Music You Want To Play");```

In The Example Case its

```AudioManager.instance.PlayMusic ("Music01");```

## Play SFX With Code

In The Function Where You Want To Play The Sound/SFX Add This Line Of Code.

```AudioManager.instance.PlaySound ("KEY Of That Sound You Want To Play);```

In The Example Case its

```AudioManager.instance.PlaySound ("Bounce");```

