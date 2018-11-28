#include <Arduino.h>
#include <DHT.h>
#include <Adafruit_Sensor.h>
#include <avr/sleep.h>

#define DHTTYPE DHT11 // DHT 11
#define DHTPIN 7      // Pin which is connected to the DHT sensor.

DHT dht(DHTPIN, DHTTYPE);
// This variable is made volatile because it is changed inside an interrupt function
// volatile int sleep_count = 0;                // Keep track of how many sleep cycles have been completed.
// const int interval = 1;                    // Interval in minutes between waking and doing tasks.
// const int sleep_total = (interval * 16) / 8; // Approximate number of sleep cycles
// needed before the interval defined above elapses. Not that this does integer math


// void watchdogOn()
// {
//   // Clear the reset flag, the WDRF bit (bit 3) of MCUSR.
//   MCUSR = MCUSR & B11110111;
//   // Set the WDCE bit (bit 4) and the WDE bit (bit 3) of WDTCSR.
//   WDTCSR = WDTCSR | B00011000;
//   // Set the watchdog timeout prescaler value to 1024 K
//   // which will yeild a time-out interval of about 8.0 s.
//   WDTCSR = B00100001;
//   // Enable the watchdog timer interupt.
//   WDTCSR = WDTCSR | B01000000;
//   MCUSR = MCUSR & B11110111;
// }

// void goToSleep()
// {
//   set_sleep_mode(SLEEP_MODE_PWR_DOWN); // Set sleep mode.
//   sleep_enable();                      // Enable sleep mode.
//   sleep_mode();                        // Enter sleep mode.
//   // After waking from watchdog interrupt the code continues
//   // to execute from this point.
//   sleep_disable(); // Disable sleep mode after waking.
//   delay(70);
//               // wait for a tenth of a second
// }

// ISR(WDT_vect)
// {
//   sleep_count++; // keep track of how many sleep cycles have been completed.
// }

void setup()
{
  Serial.begin(9600);
  // watchdogOn();
  // ADCSRA = ADCSRA & B01111111;
  // // Initialize device.
   dht.begin();
  // // Print temperature sensor details.
   pinMode(LED_BUILTIN, OUTPUT);      // set pin mode
  
}

void loop()
{
  delay(600000); //goToSleep();
 
  // if (sleep_count == sleep_total)
  // {
    // Reading temperature or humidity takes about 250 milliseconds!
    // Sensor readings may also be up to 2 seconds 'old' (its a very slow sensor)
    float h = dht.readHumidity();
    // Read temperature as Celsius (the default)
    float t = dht.readTemperature();

    // Check if any reads failed and exit early (to try again).
    if (isnan(h) || isnan(t))
    {
      Serial.println("Failed to read from DHT sensor!");
      return;
    }

    Serial.print("{ \"Temp\" :  ");
    Serial.print(t);
    Serial.print(", \"Humidity\" :  ");
    Serial.print(h);
    Serial.println("}");
    Serial.flush();

   digitalWrite(LED_BUILTIN, HIGH);   // turn the LED on (HIGH is the voltage level)
   delay(500);                // wait for a tenth of a second
   digitalWrite(LED_BUILTIN, LOW);    // turn the LED off by making the voltage LOW


  // }
}


