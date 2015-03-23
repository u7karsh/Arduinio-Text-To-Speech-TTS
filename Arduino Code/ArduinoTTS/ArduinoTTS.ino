// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Author: Utkarsh Mathur
// Email: u7karsh@yahoo.co.in
// Website: u7karsh.com
// Date: 23 March 2015
// Code: ArduinoTTS (arduino part)

#define BAUD 115200

void setup(){
  Serial.begin(BAUD);
}

void sendText( String text ){
  char out[259];
  //header
  String header = "$>";
  int data_length = text.length();
  int checksum = 0;
  checksum = (checksum ^ data_length );
  for( int i=0; i<data_length; i++){
    checksum = checksum ^ ( (int)text[i] );
  }
  
  (header + (char)data_length + text + checksum).toCharArray(out, 4 + data_length);
  Serial.println( out );
  
}

void loop(){
  sendText("Hello World!");
  delay(10000);
}
