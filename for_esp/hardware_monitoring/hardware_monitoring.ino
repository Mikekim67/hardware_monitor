#include <Wire.h>               // Only needed for Arduino 1.6.5 and earlier
#include "SSD1306Wire.h"        // legacy: #include "SSD1306.h"
// http://arduino.esp8266.com/stable/package_esp8266com_index.json 보드 추가
// esp8266 and esp32 oled driver for ssd1306 displays 라이브러리 설치

#include "Arimo_Regular_10.h"    // fonts, http://oleddisplay.squix.ch/
#include "Arimo_Regular_12.h"
#include "Arimo_Regular_14.h"
#include "Arimo_Regular_16.h" 

SSD1306Wire display(0x3c, D5, D6);

const int numRows = 7;      // 저장할 줄 수 
// 0 Time
// 1 CPU name
// 2 CPU Load
// 3 CPU Temperature
// 4 GPU name
// 5 GPU Temperature 
// 6 GPU Load
const int numCols = 50;     // 각 줄의 최대 문자 수
String data[numRows];       // 각 줄 데이터를 저장할 배열
int currentRow = 0;         // 현재 줄 인덱스

void setup() 
{
  Serial.begin(115200);
  Serial.println();

  // Initialising the UI will init the display too.
  display.init();

  display.flipScreenVertically();
  display.setTextAlignment(TEXT_ALIGN_LEFT);
  currentRow = 0;
}

void loop() 
{
  if(Serial.available()>0)
  {
    //이 부분에 코드 시작시 항상 data[0]자리부터 채워지게끔 하기 
    //setup단계에서 currentRow = 0; 추가?
    String receivedData  = Serial.readStringUntil('\n'); 
    data[currentRow] = receivedData;
    currentRow++;
    /*
    if (currentRow >= numRows) // 학교컴
    {
    // clear the display
    display.clear();
    display.setFont(Arimo_Regular_16);
    display.drawString(0, 0, data[0]); //Time

    char buffer[256];
    display.setFont(Arimo_Regular_10);
    display.drawStringf(0, 16, buffer, "%s | %s",data[1], data[4]); 

    display.setFont(Arimo_Regular_14);
    display.drawStringf(0, 32, buffer, "CPU: %s | %s",data[3], data[2]);
    display.drawStringf(0, 48, buffer, "GPU: %s | %s",data[5], data[6]);
 
    currentRow = 0;
    // write the buffer to the display
    display.display();
    delay(10);
    }
    */
     if (currentRow >= numRows) // 집컴, data[0]자리에 data[6]이 와있음
    { // c#코드상에서 정렬하고 데이터 보내도록 하기
    // clear the display
    display.clear();
    display.setFont(Arimo_Regular_16);
    display.drawString(0, 0, data[1]); //Time

    char buffer[256];
    display.setFont(Arimo_Regular_10);
    display.drawStringf(0, 16, buffer, "%s | %s",data[2], data[5]); 

    display.setFont(Arimo_Regular_14);
    display.drawStringf(0, 32, buffer, "CPU: %s | %s",data[4], data[3]);
    display.drawStringf(0, 48, buffer, "GPU: %s | %s",data[6], data[0]);
 
    currentRow = 0;
    // write the buffer to the display
    display.display();
    delay(10);
    }
  }
}