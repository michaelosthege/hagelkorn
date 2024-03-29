[![PyPI version](https://img.shields.io/pypi/v/hagelkorn)](https://pypi.org/project/hagelkorn)
[![NuGet](https://img.shields.io/nuget/v/hagelkorn)](https://nuget.org/packages/Hagelkorn)
[![pipeline](https://github.com/michaelosthege/hagelkorn/workflows/pipeline/badge.svg)](https://github.com/michaelosthege/hagelkorn/actions)
[![coverage](https://codecov.io/gh/michaelosthege/hagelkorn/branch/master/graph/badge.svg)](https://codecov.io/gh/michaelosthege/hagelkorn)
[![DOI](https://zenodo.org/badge/338276869.svg)](https://zenodo.org/badge/latestdoi/338276869)

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

## Installation
The Python version is available [on PyPI](https://pypi.org/packages/hagelkorn):

```bash
pip install hagelkorn
```

The C#/.NET Standard package is available [on NuGet](https://nuget.org/packages/Hagelkorn).
