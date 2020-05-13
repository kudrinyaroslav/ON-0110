import sys
import re
import os
import codecs
import subprocess

def revision(path):
    proc = subprocess.Popen(['svn','info', path],stdout=subprocess.PIPE)
    header = "Last Changed Rev: "
    for line in proc.stdout:
        try:
            v = line.rstrip().decode("utf-8")
            if v.startswith(header):
                return v[len(header):]
                break
        except:
            pass

revision = revision(sys.argv[1])
print("Revision: " + revision)

files = sys.argv[2].split(';')

def substitutions():
    corePattern, coreReplace = "(\w+)\.(\w+)\.(\w+)\.\w+", "\g<1>.\g<2>.\g<3>.%s"
    for (header, footer) in [('[assembly: AssemblyVersion("', '")]'), ('[assembly: AssemblyFileVersion("', '")]')]:
        search     = lambda: re.escape(header) + '%s' + re.escape(footer)
        substitute = lambda: header + '%s' + footer

        yield search() % corePattern, (substitute() % coreReplace) % revision

def replace(filenameIn, subs):
    filenameOut = filenameIn + ".tmp"
    with codecs.open(filenameIn, encoding = "utf-8") as fileIn:
        lines = [line for line in fileIn]

    for (p, s) in subs:
        lines = [re.sub(p, s, line) for line in lines]

    with codecs.open(filenameIn, "w", encoding = "utf-8") as fileOut:
        for line in lines:
            fileOut.write(line)


for filenameIn in files:
    replace(filenameIn, substitutions())
