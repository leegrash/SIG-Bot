int motorv1 = 3;
int motorh1 = 10;
int received = 75;
int hastighet = 15;
int motorfram = 0;
int motorbak = 0;

void setup() {
 pinMode(motorv1, OUTPUT);
 pinMode(motorh1, OUTPUT);
 Serial.begin(9600);
}
void loop() {
  while(Serial.available()) {
   received = Serial.read();
  if(received == 1){  
    digitalWrite(motorv1, hastighet);
    digitalWrite(motorh1, hastighet);
  }
  else if(received == 2){  
    digitalWrite(motorv1, 0);
    digitalWrite(motorh1, 0);
  }
 }
}
