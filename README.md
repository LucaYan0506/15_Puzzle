# 15_Puzzle 
I re-did a famous game called 15 puzzle using C#.  

My record is 40 seconds.  
Download the game and try to beat me!  
![image](https://user-images.githubusercontent.com/83918638/221365559-3515beaf-c4e9-4045-a3b2-4d97a292125e.png)

## How to download the game
Click [here](https://github.com/LucaYan0506/15_Puzzle/releases/download/v1.0.0/installer.exe) to download the file  

#### Chrome
If you are using Google Chrome, it may block download.   
![image](https://github.com/LucaYan0506/Binary-code-Puzzle/blob/master/screenshot/Screenshot%202022-02-21%20202953.jpg)    
To fix this problem, please click arrow up symbol (as shown in the image below) then click "keep".  
![image](https://github.com/LucaYan0506/Binary-code-Puzzle/blob/master/screenshot/Screenshot%202022-02-21%20201656.jpg)  

#### Edge
If you are using Microsoft Edge, it may block download.   
![image](https://github.com/LucaYan0506/Binary-code-Puzzle/blob/master/screenshot/Screenshot%202022-02-21%20202803.jpg)  
To fix this problem, please click arrow up symbol (as shown in the image below) then click "keep".  
![image](https://github.com/LucaYan0506/Binary-code-Puzzle/blob/master/screenshot/Screenshot%202022-02-21%20202859.jpg)  
After the donwload is finished click it  
![image](https://user-images.githubusercontent.com/83918638/155171074-a1149aef-6142-4513-81e8-4eeeb3a12ed4.png)   
Microsoft Defender and other antivirus may block it because this app is an unpopular application. Click "More info"  
![image](https://user-images.githubusercontent.com/83918638/155171920-3f0ad496-f25a-4735-8e3b-4eb4617dfd01.png)  
then click "Run anyway"（rest assured, it is a safe app, all code can be seen [here](#))  
![image](https://user-images.githubusercontent.com/83918638/155171870-ee4f4330-7a32-4890-9c01-1deaccd2da12.png)   
## How to let the AI solve it
1) Click here  
![image](https://user-images.githubusercontent.com/83918638/221365945-06006399-c78b-48f9-a448-27f94377fd73.png)
2) There are 2 options: Beginner and Advanced AI. Choose one of them and click it.    
![image](https://user-images.githubusercontent.com/83918638/221366044-e04faf21-66e6-45c3-9b04-e7eba9acce05.png)
## Theory behind Beginner AI
The whole algorithm is based on DFS. Using DFS it tries through every possible combination.  
![image](https://user-images.githubusercontent.com/83918638/221366654-6890d31c-3b63-47d6-ab54-98d0e9a3c782.png)  
To speed up the algorithm, I've decided to "lock" cells when they are in the correct side/position.
![image](https://user-images.githubusercontent.com/83918638/221367405-b15cdd67-f7ab-4a3e-a7cd-ac5d003d7e51.png)

## Theory behind Advanced AI
The initial idea was using A-Star algorithm to find the quickest path and solve it in less than 10 seconds. However, due the limitation of the hardware, if I use A-Star it will take ages to find the quickest path. So, I've used DFS for the first part and A-Star for the second part. In order to speed up the first part I've used the same approch for Beginner AI + an reduceMoves algorithm. This algorithm will skip some moves.  
![image](https://user-images.githubusercontent.com/83918638/221367835-5ca328c8-2fa4-4b42-b773-ef818af21ec7.png)
![image](https://user-images.githubusercontent.com/83918638/221367819-679b16c2-138f-423f-8ebd-ae66f4a89354.png)

