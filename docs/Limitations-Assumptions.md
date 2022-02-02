# Limitations and Assumptions

- **Assumption:** DataSources are 1:1 with DataValues
  - ex. Tag Definition -> Tag Value
  - May not always be true
    - ex. Read from RS232 -> returns several values
- **Limitation:** WaitForInterval is not a separate thread 
  - This is fine for a timer, but not for polling a rising edge/etc
  - This is also a performance bottleneck that should be optimized out as more sources and targets are developed
- **Assumption:** We don't loop the acquisition
  - Once a start trigger occurs, we run, wait for an end trigger, and then exit
  - We don't loop and wait for a start trigger again