import os
import shutil

def unpack_iso():
    try:
        import pycdlib
    except:
        os.system("pip install pycdlib")
        import pycdlib
    print("Making isofiles directory")
    os.makedirs("./isofiles", exist_ok=True)
    iso_name = None
    for file in os.listdir(os.getcwd()):
        if file.endswith('.iso'):
            print("Found: " + file) 
            iso_response = input("Is this the correct original .iso? (Y/N) ")
            if iso_response.lower() == 'y':
                iso_name = file
                break
    if iso_name == None:
        print("Could not find .iso! Exiting.")
        quit()
    if not os.path.exists("Shining_Blade_PATCHED.iso"):
        shutil.copy(iso_name,"Shining_Blade_PATCHED.iso")
    
    print("Extracting files from original iso...")
    iso = pycdlib.PyCdlib()
    iso.open(iso_name)
    print("Extracting ICON0.PNG")
    iso.get_file_from_iso("isofiles/ICON0.PNG", iso_path='/PSP_GAME/ICON0.PNG')
    print("Extracting DATA.BIN")
    iso.get_file_from_iso("isofiles/DATA.BIN", iso_path='/PSP_GAME/INSDIR/DATA.BIN')
    print("Extracting EBOOT.BIN")
    iso.get_file_from_iso("isofiles/EBOOT.BIN", iso_path='/PSP_GAME/SYSDIR/EBOOT.BIN')
    iso.close()

def insert_iso():
    print("Iso file...")
    os.system(r"bin\Umd-replace\UMD-replace.exe Shining_Blade_PATCHED.iso PSP_GAME\INSDIR\DATA.BIN isofiles\DATA.cpk.encrypt")
    os.system(r"bin\Umd-replace\UMD-replace.exe Shining_Blade_PATCHED.iso PSP_GAME\SYSDIR\EBOOT.BIN isofiles\EBOOT_PATCHED.BIN")
    os.system(r"bin\Umd-replace\UMD-replace.exe Shining_Blade_PATCHED.iso PSP_GAME\ICON0.PNG  isofiles\ICON0.PNG")
    
def decrypt_eboot():
    print("Decrypting eboot...")
    os.system("bin\\DecEboot\\deceboot.exe isofiles/EBOOT.BIN isofiles/EBOOT_DEC.BIN")
def pgdecrypt():
    cwd = os.getcwd()
    shutil.copy("bin\\pgdecrypt\\pgdecrypt.exe","isofiles/pgdecrypt.exe")
    os.chdir("isofiles/")
    os.system("pgdecrypt.exe")
    os.chdir(cwd)
def unpack_cpk():
    print("Unpack cpk...")
    os.system("bin\\crifilesystem\\cpkmakec.exe isofiles/DATA.BIN.decrypt -extract=isofiles/DATA_extract")
def repack_cpk():
    print("Repack cpk...")
    os.system("bin\\crifilesystem\\cpkmakec.exe csv/DATA.csv isofiles/DATA.CPK -mode=FILENAMEID")
def decode_file():
    os.system("decode.bat")
def encode_file():
    os.system("encode.bat")
def insert_asm():
    print("Insert asm...")
    os.system(r"bin\Armips\armips.exe asm\EBOOT.asm")
