
import datetime
import enum
import random as rnd

DEFAULT_ALPHABET = '13456789ABCDEFHKLMNPQRTWXYZ'


class Resolution(object):
    """Helper type for specifying minimum time resolutions."""
    microseconds = 1e-6
    milliseconds = 1e-3
    seconds = 1
    minutes = 60
    hours = 3600
    days = 86400


def key_length(overflow_years:float, resolution:float, B:int):
    """
    Determines some key parameters for ID generation.

    Arguments:
        overflow_years (float): number of years after which the key length will be exceeded
        resolution (float): maximum length of an interval (in seconds)
        B (int): base of the positional notation (length of alphabet)

    Returns:
        D (int): number of digits of the ID
        K (int): total number of unique IDs (intervals)
        T (float): duration of one interval in seconds
    """
    total_seconds = overflow_years * 31536000
    K_min = total_seconds / resolution
    D = 1
    K = B
    while K < K_min:
        D += 1
        K *= B
    T = total_seconds / K
    return D, K, T


def base(n:float, alphabet:str, digits:int):
    """
    Converts a real-valued number into its baseN-notation.

    Arguments:
        n (float): number to be converted (decimal precision will be droped)
        alphabet (str): alphabet of the positional notation system
        digits (int): number of digits in the ID

    Returns:
        id (str): length may exceed the specified number of digits if n results in an overflow
    """
    B = len(alphabet)
    output = ''
    while n > 0:
        output += alphabet[n % B]
        n  = n // B
    return output[::-1].rjust(digits, alphabet[0])


class HagelSource(object):
    """An ID-generator that exposes some internal parameters."""
    def __init__(self,
                 resolution=Resolution.seconds,
                 alphabet=DEFAULT_ALPHABET,
                 start=datetime.datetime(2018, 1, 1, tzinfo=datetime.timezone.utc),
                 overflow_years=10,
                 ):
        """Creates an ID-generator that is slightly faster and a bit more transparent.

        Arguments:
            resolution (float): maximum duration in seconds for an increment in the id
            alphabet (str): the (sorted) characters to be used in the ID generation
            start (datetime): beginning of timeline
            overflow_years (float): number of years after which the key length will increase by 1
        """
        self.alphabet = alphabet
        self.B = len(alphabet)
        self.start = start.astimezone(datetime.timezone.utc)
        self.total_seconds = overflow_years * 31536000
        self.end = self.start + datetime.timedelta(self.total_seconds / 86400)

        self.digits, self.combinations, self.resolution = key_length(overflow_years,
                                                                     resolution, self.B)

        return super().__init__()

    def monotonic(self, now=None):
        """
        Generates a short, human-readable ID that increases monotonically with time.

        Arguments:
            now (datetime): timpoint at which the ID is generated

        Returns:
            id (str)
        """
        if now == None:
            now = datetime.datetime.utcnow()

        elapsed_seconds = (now.astimezone(datetime.timezone.utc) - self.start).total_seconds()
        elapsed_intervals = int(elapsed_seconds / self.resolution)

        return base(elapsed_intervals, self.alphabet, self.digits)


def monotonic(resolution=Resolution.seconds,
        now:datetime.datetime=None,
        alphabet=DEFAULT_ALPHABET,
        start=datetime.datetime(2018, 1, 1, tzinfo=datetime.timezone.utc),
        overflow_years=10,
        ):
    """
    Generates a short, human-readable ID that increases monotonically with time.

    Arguments:
        resolution (float): maximum duration in seconds for an increment in the id
        now (datetime): timpoint at which the ID is generated
        alphabet (str): the (sorted) characters to be used in the ID generation
        start (datetime): beginning of timeline
        overflow_years (float): number of years after which the key length will increase by 1

    Returns:
        id (str)
    """
    # clean up input arguments
    start = start.astimezone(datetime.timezone.utc)
    if now == None:
        now = datetime.datetime.utcnow()

    # find parameters
    B = len(alphabet)
    digits, combinations, resolution = key_length(overflow_years, resolution, B)

    # find the interval number
    elapsed_seconds = (now.astimezone(datetime.timezone.utc) - start).total_seconds()
    elapsed_intervals = int(elapsed_seconds / resolution)

    # encode
    return base(elapsed_intervals, alphabet, digits)


def random(digits:int=5, alphabet:str=DEFAULT_ALPHABET):
    """
    Generates a random alphanumberic ID.

    Arguments:
        digits (int): length of the generated ID
        alphabet (str): available characters for the ID

    Returns:
        id (str)
    """
    return ''.join(rnd.choices(alphabet, k=digits))
