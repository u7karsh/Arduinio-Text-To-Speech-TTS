# Arduinio-Text-To-Speech-TTS

Speech to text synthesis using SAPI with C#. String to be synthesized is transmitted over serial link from arduino.

##Datagram Protocol

###Header construction

The header consists of 3 characters "$>" (preamble) appended by a single byte signifying number of bytes in original bytes.

Thus header = preamble + length of message

###Tail construction

The tail consists of a single byte containing a checksum byte which is formed by XORing all bytes leaving preamble part.

Thus,

Datagram = preamble + length of message + message + checksum

##Arduino Code Edits

The code contains a function sendText that requires a string that needs to be synthesized

#Usage

1. Flash the arduino code in arduino 
2. Editing com port string in C# code. 
3. Run the C# code