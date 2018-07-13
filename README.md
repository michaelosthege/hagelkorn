# Hagelkorn
This is a package for generating human-readable and human-memorable IDs.

Aside from random ID-generation from a reduced alphabet, IDs can also be generated such that they
are monotonically increasing with time.

## Usage

```python
import hagelkorn

# default settings sample about 17 million different IDs
hagelkorn.random()
> '4W8K6'

hagelkorn.monotonic()
> '1DFL5M'

# time-resolution of < 0.1 seconds
# overflowing after exactly 5 years
hagelkorn.monotonic(resolution=0.1, overflow_years=5)
> '2Y38H6C'
```