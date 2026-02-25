import sys
sys.path.insert(0, '.')

from helpers.app import launch_app, get_window
import time

app = launch_app()
win = get_window(app)
print(win.print_control_identifiers())
win.UsernameEdit.type_keys("user")
win.PasswordEdit.type_keys("password")
win.SignInButton.click()

